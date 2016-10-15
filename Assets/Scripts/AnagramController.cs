using System.Collections;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	[System.Serializable]
	public sealed class AnagramController
	{
		public AnagramView view;
		public LevelSelectController levelSelect = new LevelSelectController();
		public AnagramModel model = new AnagramModel();
		public bool isVerbose = true;
		public Storage storage = new Storage();
		public HintController hint;
		private Email email = new Email();

		private ButtonController buttonController = new ButtonController();
		private string buttonDownName;
		// Select letter:
		private int letterIndexMouseDown;

		public void Setup()
		{
			Words.Setup(model);
			model.Setup();
			model.ScaleToScreen(9.5f);
			model.Load(storage.Load());
			if (null == view)
			{
				return;
			}
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
			LevelSelectSetup();
			hint.Setup();
			hint.model = model.hint;
		}

		private void LevelSelectSetup()
		{
			levelSelect.model.levelUnlocked = model.progress.levelUnlocked;
			levelSelect.model.levelCount = model.progress.levelMax;
			levelSelect.model.menus = new List<int>(){8, 20, 20};
			levelSelect.model.menuNames = new List<string>(){
				"chapterSelect",
				"levelSelect",
				"wordSelect",
				"play"
			};
			levelSelect.Setup();
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
			buttonController.view.Listen(view.exitButton);
			buttonController.view.Listen(view.emailButton);
			buttonController.view.Listen(view.deleteStorageButton);
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

		private void UpdateExit()
		{
			if ("exit" == buttonDownName) {
				model.Pause();
				levelSelect.model.Exit();
			}
		}

		//
		// Remember which letter was just clicked on this update.
		//
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
				letterIndexMouseDown = StringUtil.ParseIndex(name);
				DebugUtil.Log("AnagramController.UpdateLetterButton: " + buttonDownName);
			}
		}

		public void Update(float deltaSeconds)
		{
			if ("store" == hint.model.state) {
				deltaSeconds = 0.0f;
			}
			UpdateLevelSelect();
			UpdateSave();
			UpdateExit();
			UpdateButtonController();
			UpdateCheat();
			if (model.isGamePlaying) {
				UpdateSelectLetter();
			}
			UpdateBackspace();
			UpdateContinue();
			UpdateNewGame();
			model.Update(deltaSeconds);
			UpdateLetters();
			UpdateHud();
			UpdateHint();
			UpdateScreenOrientation();
			UpdatePosition();
		}

		private void UpdateLevelSelect()
		{
			levelSelect.model.levelUnlocked = model.progress.levelUnlocked;
			levelSelect.model.levelCurrently = model.progress.level;
			levelSelect.Update();
			if (levelSelect.model.inMenu.IsChangeTo(false))
			{
				model.SelectLevel(levelSelect.model.levelSelected);
			}
		}

		public void UpdateDeleteStorage()
		{
			if (view.deleteStorageButton == buttonController.view.target)
			{
				DebugUtil.Log("AnagramController.Deleting storage.");
				storage.Delete();
			}
		}

		public void Save()
		{
			storage.SetKeyValue("level", model.progress.GetLevelNormal());
			storage.SetKeyValue("hint", model.hint.count);
			storage.SetKeyValue("cents", model.hint.cents);
			storage.Save(storage.data);
		}

		private void UpdateSave()
		{
			if (model.isSaveNow)
			{
				model.isSaveNow = false;
				Save();
			}
		}

		//
		// http://answers.unity3d.com/questions/762073/c-list-of-string-name-for-inputgetkeystring-name.html
		// 
		private void UpdateCheat()
		{
			if (KeyView.IsDownNow("page up"))
			{
				model.select.inputs = DataUtil.Split(model.text, "");
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
			UpdateDeleteStorage();
		}

		private void UpdateLetters()
		{
			TextViews.SetChildren(view.wordLetters, model.select.word);
			TextViews.SetChildren(view.inputLetters, model.select.inputs);
			TextViews.SetChildren(view.outputLetters, model.outputs);
			TextViews.SetChildren(view.hintLetters, model.hint.reveals);
		}

		private void UpdateSelectLetter()
		{
			string input = KeyView.InputString();
			model.UpdateSelect(input, letterIndexMouseDown);
			if ("complete" != model.state)
			{
				UpdateSelect(model.select.selectedIndexes.selectsNow, true);
			}
			UpdateSubmit();
		}

		// Delete or backspace:  Remove last letter.
		private void UpdateBackspace()
		{
			if (KeyView.IsDownNow("delete")
			|| KeyView.IsDownNow("backspace")
			|| "delete" == buttonDownName)
			{
				model.select.Pop();
				model.Backspace();
			}
			UpdateSelect(model.select.selectedIndexes.removesNow, false);
		}

		// Each selected letter in word plays animation "selected".
		// Select, submit: Anders sees reticle and sword. Test case:  2015-04-18 Anders sees word is a weapon.
		private void UpdateSelect(List<int> selects, bool selected)
		{
			string state = selected ? "selected" : "none";
			for (int s = 0; s < selects.Count; s++)
			{
				int index = (int) selects[s];
				AnimationView.SetState(view.wordLetters[index], state);
				if (isVerbose)
				{
					DebugUtil.Log("AnagramController.UpdateSelect: " 
						+ index + ": " + state);
				}
			}
		}

		private void ResetSelect()
		{
			SetLetterStates("none");
		}

		private void SetLetterStates(string state)
		{
			for (int i = 0; i < DataUtil.Length(view.wordLetters); i++)
			{
				AnimationView.SetState(view.wordLetters[i], state);
			}
		}

		internal void Pause(bool isInstant)
		{
			model.Pause(isInstant);
			Update(0.0f);
		}

		private void UpdateHud()
		{
			string hudState = model.isHudVisible ? "begin" : "end";
			AnimationView.SetState(view.hud, hudState, false, true);
			if (null != model.helpTextNow.next)
			{
				TextView.SetText(view.helpText, model.helpTextNow.next);
			}
			if (null != model.helpStateNow)
			{
				AnimationView.SetTrigger(view.help, model.helpStateNow);
			}
			TextView.SetText(view.score, model.score.ToString());
			int levelNumber = model.progress.level + 1;
			TextView.SetText(view.level, levelNumber.ToString());
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
			if (model.wordState.IsChange())
			{
				AnimationView.SetState(view.wordState, model.wordState.next, true);
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
				if ("complete" == model.state)
				{
					SetLetterStates("complete");
				}
				else
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
			hint.Update();
			if (KeyView.IsDownNow("?")
			|| KeyView.IsDownNow("/")
			|| "hint" == buttonDownName)
			{
				model.Hint();
			}
			SceneNodeView.SetVisible(view.hintButton, model.hint.isVisible);
			TextView.SetText(view.hintText, model.hint.GetText());

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
		}
	}
}
