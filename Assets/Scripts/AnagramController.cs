using System.Collections;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramController
	{
		public AnagramView view;
		internal AnagramModel model = new AnagramModel();
		private Email email = new Email();
		private Storage storage = new Storage();
		private string buttonDownName;
		private int letterIndexMouseDown;
		private ButtonController buttonController = new ButtonController();

		public void Setup()
		{
			model.Setup();
			Words.Setup(model);
			model.ScaleToScreen(9.5f);
			model.Load(storage.Load());
			model.SetReadingOrder(view.letterNodes);
			view.wordLetters = SceneNodeView.GetChildrenByPattern(view.wordState, "bone_{0}/letter", model.letterMax);
			view.wordBones = SceneNodeView.GetChildrenByPattern(view.wordState, "bone_{0}", model.letterMax);
			view.inputLetters = SceneNodeView.GetChildrenByPattern(view.inputState, "Letter_{0}", model.letterMax);
			view.outputLetters = SceneNodeView.GetChildrenByPattern(view.output, "Letter_{0}", model.letterMax);
			view.hintLetters = SceneNodeView.GetChildrenByPattern(view.hint, "Letter_{0}", model.letterMax);
			view.tweenSwap.Setup(view.wordBones);
			SetupButtonController();
			ScreenView.AutoRotate();
			UpdateScreenOrientation();

		}

		private void UpdateScreenOrientation()
		{
			string aspect = ScreenView.IsPortrait() ? "portrait" : "landscape";
			AnimationView.SetState(view.main, aspect);
		}

		private void SetupButtonController()
		{
			buttonController.view.Listen(view.hintButton);
			buttonController.view.Listen(view.newGameButton);
			buttonController.view.Listen(view.continueButton);
			buttonController.view.Listen(view.deleteButton);
			buttonController.view.Listen(view.submitButton);
			buttonController.view.Listen(view.emailButton);
			for (int index = 0; index < DataUtil.Length(view.wordLetters); index++)
			{
				var letter = view.wordLetters[index];
				buttonController.view.Listen(letter);
			}
		}

		private void UpdateButtonController()
		{
			buttonDownName = null;
			buttonController.Update();
			if (buttonController.isAnyNow) {
				buttonDownName = buttonController.downName;
			}
			UpdateLetterButton();
		}

		//
		// * Remember which letter was just clicked on this update.
		// *
		// http://answers.unity3d.com/questions/20328/onmousedown-to-return-object-name.html
		// 
		private void UpdateLetterButton()
		{
			letterIndexMouseDown = -1;
			if ("letter" == buttonController.downName && null != buttonController.view.target) {
				var text = SceneNodeView.GetChild(buttonController.view.target, "text");
				buttonDownName = TextView.GetText(text).ToLower();
				string name = SceneNodeView.GetName(
					SceneNodeView.GetParent(buttonController.view.target));
				letterIndexMouseDown = Toolkit.ParseIndex(name);
				Toolkit.Log("AnagramController.UpdateLetterButton: " + buttonDownName);
			}
		}

		public void Update(float deltaSeconds)
		{
			model.Update(deltaSeconds);
			if (model.isSaveNow) {
				model.isSaveNow = false;
				storage.SetKeyValue("level", model.progress.GetLevelNormal());
				storage.Save(storage.data);
			}
			UpdateButtonController();
			UpdateBackspace();
			UpdateCheat();
			if (model.isGamePlaying) {
				UpdatePlay();
			}
			UpdateLetters();
			UpdateHud();
			UpdateHint();
			UpdateContinue();
			UpdateNewGame();
			UpdateScreenOrientation();
		}

		private void UpdateLetters()
		{
			TextViews.SetChildren(view.wordLetters, model.word);
			TextViews.SetChildren(view.inputLetters, model.inputs);
			TextViews.SetChildren(view.outputLetters, model.outputs);
			TextViews.SetChildren(view.hintLetters, model.hints);
		}

		public bool IsLetterKeyDown(string letter)
		{
			return KeyView.IsDownNow(letter.ToLower());
		}

		private void UpdatePlay()
		{
			List<string> presses = model.GetPresses(IsLetterKeyDown);
			model.Press(presses);
			model.MouseDown(letterIndexMouseDown);
			UpdateSelect(model.selectsNow, true);
			UpdateSubmit();
			UpdatePosition();
		}

		//
		// http://answers.unity3d.com/questions/762073/c-list-of-string-name-for-inputgetkeystring-name.html
		// 
		private void UpdateCheat()
		{
			if (KeyView.IsDownNow("page up"))
			{
				model.inputs = DataUtil.Split(model.text, "");
				buttonDownName = "submit";
			}
			else if (KeyView.IsDownNow("page down"))
			{
				model.LevelDownMax();
			}
			else if (KeyView.IsDownNow("0")
					|| "email" == buttonDownName)
			{
				string message = model.metrics.ToTable();
				message += "\n\n\n";
				message += model.journal.Write();
				email.Send(message);
			}
		}

		//
		// Delete or backspace:  Remove last letter.
		// 
		private void UpdateBackspace()
		{
			if (KeyView.IsDownNow("delete")
			|| KeyView.IsDownNow("backspace")
			|| "delete" == buttonDownName)
			{
				model.Backspace();
				buttonDownName = null;
			}
			UpdateSelect(model.backspacesNow, false);
		}

		//
		// Each selected letter in word plays animation "selected".
		// Select, submit: Anders sees reticle and sword. Test case:  2015-04-18 Anders sees word is a weapon.
		// Could cache finds.
		// 
		private void UpdateSelect(List<int> selects, bool selected)
		{
			string state = selected ? "selected" : "none";
			for (int s = 0; s < selects.Count; s++)
			{
				int index = (int) selects[s];
				AnimationView.SetState(view.wordLetters[index],
					state);
			}
		}

		private void UpdateHud()
		{
			string hudState = model.isHudVisible ? "begin" : "end";
			AnimationView.SetState(view.hud, hudState, false, true);
			if ("" != model.help)
			{
				TextView.SetText(view.helpText, model.help);
			}
			if (model.isHelpStateChange)
			{
				string helpState = "" == model.help ? "endNow" : "beginNow";
				AnimationView.SetTrigger(view.help, helpState);
			}
			// SceneNodeView.SetVisible(view.help, model.help != "");
			TextView.SetText(view.score, model.score.ToString());
			TextView.SetText(view.level, model.progress.level.ToString());
			TextView.SetText(view.levelMax, model.progress.levelMax.ToString());
		}

		//
		// Press space or enter.  Input word.
		// Word robot approaches.
		// 	 restructure synchronized animations:
		// 		 word
		// 			 complete
		// 			 state
		// 		 input
		// 			 output
		// 			 state
		// Play "complete" animation on word, so that letters are off-screen when populating next word.
		// 
		private void UpdateSubmit()
		{
			if (null != model.wordStateNow)
			{
				AnimationView.SetState(view.wordState, model.wordStateNow, true);
				model.wordStateNow = null;
			}
			if (KeyView.IsDownNow("space")
			|| KeyView.IsDownNow("return")
			|| "submit" == buttonDownName)
			{
				Submit();
			}
			if ("submit" == model.journal.actionNow && null != model.state) 
			{
				AnimationView.SetState(view.input, model.state, true);
				if ("complete" != model.state)
				{
					ResetSelect();
				}
			}
			string completedNow = AnimationView.CompletedNow(view.input);
			if ("complete" == completedNow)
			{
				model.TrialComplete();
			}
			if ("trialComplete" == model.journal.actionNow)
			{
				view.tweenSwap.Reset();
				ResetSelect();
			}
		}

		public void Submit()
		{
			buttonDownName = null;
			model.Submit();
		}

		private void UpdateOutputHitsWord()
		{
			if (model.OnOutputHitsWord())
			{
				if ("complete" != model.state)
				{
					view.tweenSwap.Move(model.stationIndexes);
				}
			}
		}

		//
		// Hint does not reset letters selected.
		// Test case:  2016-06-19 Hint.  Expect no mismatch between letters typed and letters selected.
		// 
		private void UpdateHint()
		{
			if (KeyView.IsDownNow("?")
			|| KeyView.IsDownNow("/")
			|| "hint" == buttonDownName)
			{
				model.Hint();
			}
			SceneNodeView.SetVisible(view.hintButton, model.isHintVisible);

		}

		private void UpdateNewGame()
		{
			if (KeyView.IsDownNow("home")
			|| "newGame" == buttonDownName)
			{
				model.NewGame();
			}
			if ("newGame" == model.journal.actionNow)
			{
				ResetSelect();
			}
			SceneNodeView.SetVisible(view.newGameButton, model.isNewGameVisible);

		}

		private void UpdateContinue()
		{
			if (KeyView.IsDownNow("end")
			|| "continue" == buttonDownName)
			{
				model.ContinueGame();
			}
			if ("continue" == model.journal.actionNow)
			{
				ResetSelect();
			}
			SceneNodeView.SetVisible(view.continueButton, model.isContinueVisible);
			SceneNodeView.SetVisible(view.deleteButton, !model.isContinueVisible);
			SceneNodeView.SetVisible(view.submitButton, !model.isContinueVisible);

		}

		private void ResetSelect()
		{
			int max = model.word.Count;
			for (int index = 0; index < max; index++)
			{
				string name = "bone_" + index + "/letter";
				AnimationView.SetState(
					SceneNodeView.GetChild(view.wordState, name), "none");
			}
		}

		//
		// Multiply progress position by world scale on progress.
		// An ancestor is scaled.  The checkpoints are placed in 
		// the editor at the model's checkpoint interval (which at this time is 16).
		// Test case:  2016-06-26 Reach two checkpoints.  
		// Expect checkpoint line near bottom of screen each time.
		// Got checkpoint lines had gone down off the screen.
		// 
		private void UpdatePosition()
		{
			if (model.MayKnockback())
			{
				UpdateOutputHitsWord();
			}
			SceneNodeView.SetLocalY(view.word, model.wordPositionScaled * view.wordPositionScale);
			SceneNodeView.SetLocalY(view.progress, model.progressPositionTweened * SceneNodeView.GetWorldScaleY(view.progress));
		}
	}
}
