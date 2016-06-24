using System.Collections;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramController
	{
		private Model model;
		internal AnagramView view = new AnagramView();
		private Email email = new Email();
		private string letterMouseDown;
		private int letterIndexMouseDown;

		public void Start()
		{
			model = new Model();
		}

		/**
		 * Remember which letter was just clicked on this update.
		 *
		 * http://answers.unity3d.com/questions/20328/onmousedown-to-return-object-name.html
		 */
		private void updateMouseDown()
		{
			letterMouseDown = null;
			letterIndexMouseDown = -1;
			MouseView.Update();
			if (MouseView.target) {
				var text3d = SceneNodeView.GetChild(MouseView.target, "text3d");
				letterMouseDown = TextView.GetText(text3d).ToLower();
				string name = SceneNodeView.GetName(
					SceneNodeView.GetParent(MouseView.target));
				letterIndexMouseDown = Toolkit.ParseIndex(name);
				// Debug.Log("View.updateMouseDown: " + letterMouseDown);
			}
		}

		public void Update(float deltaSeconds)
		{
			model.update(deltaSeconds);
			updateCheat();
			updateMouseDown();
			updateBackspace();
			if (model.isGamePlaying) {
				updatePlay();
			}
			view.updateLetters(view.wordState, model.word, "bone_{0}/letter");
			view.updateLetters(view.inputState, model.inputs, "Letter_{0}");
			view.updateLetters(view.output, model.outputs, "Letter_{0}");
			view.updateLetters(view.hint, model.hints, "Letter_{0}");
			updateHud();
			updateHint();
			updateContinue();
			updateNewGame();
		}

		public bool IsLetterKeyDown(string letter)
		{
			return KeyView.IsDownNow(letter.ToLower());
		}

		private void updatePlay()
		{
			List<string> presses = model.getPresses(IsLetterKeyDown);
			updateSelect(model.press(presses), true);
			updateSelect(model.mouseDown(letterIndexMouseDown), true);
			updateSubmit();
			updatePosition();
		}

		/**
		 * http://answers.unity3d.com/questions/762073/c-list-of-string-name-for-inputgetkeystring-name.html
		 */
		private void updateCheat()
		{
			if (KeyView.IsDownNow("page up"))
			{
				model.levelUp();
				view.audio.Play("select");
			}
			else if (KeyView.IsDownNow("page down"))
			{
				model.levelDownMax();
				view.audio.Play("select");
			}
			else if (KeyView.IsDownNow("0")
					|| "email metrics" == letterMouseDown)
			{
				email.Send(model.metrics.ToTable());
			}
		}

		/**
		 * Delete or backspace:  Remove last letter.
		 */
		private void updateBackspace()
		{
			if (KeyView.IsDownNow("delete")
			|| KeyView.IsDownNow("backspace")
			|| "delete" == letterMouseDown)
			{
				updateSelect(model.backspace(), false);
				letterMouseDown = null;
			}
		}

		/**
		 * Each selected letter in word plays animation "selected".
		 * Select, submit: Anders sees reticle and sword. Test case:  2015-04-18 Anders sees word is a weapon.
		 * Could cache finds.
		 */
		private void updateSelect(List<int> selects, bool selected)
		{
			string state = selected ? "selected" : "none";
			for (int s = 0; s < selects.Count; s++)
			{
				int index = (int) selects[s];
				string name = "bone_" + index + "/letter";
				AnimationView.SetState(
					SceneNodeView.GetChild(view.wordState, name), state);
				view.audio.Play("select");
			}
		}

		private void updateHud()
		{
			SceneNodeView.SetVisible(view.hud, model.isHudVisible);
			TextView.SetText(view.help, model.help);
			TextView.SetText(view.score, model.score.ToString());
			TextView.SetText(view.level, model.progress.level.ToString());
			TextView.SetText(view.levelMax, model.progress.levelMax.ToString());
		}

		/**
		 * Press space or enter.  Input word.
		 * Word robot approaches.
		 *	 restructure synchronized animations:
		 *		 word
		 *			 complete
		 *			 state
		 *		 input
		 *			 output
		 *			 state
		 */
		private void updateSubmit()
		{
			if (KeyView.IsDownNow("space")
			|| KeyView.IsDownNow("return")
			|| "submit" == letterMouseDown)
			{
				letterMouseDown = null;
				string state = model.submit();
				if (null != state) 
				{
					// TODO AnimationView.SetState(view.wordState, state);
					AnimationView.SetState(view.input, state);
					view.audio.Play("shoot");
				}
				resetSelect();
			}
		}

		/**
		 * Hint does not reset letters selected.
		 * Test case:  2016-06-19 Hint.  Expect no mismatch between letters typed and letters selected.
		 */
		private void updateHint()
		{
			if (KeyView.IsDownNow("question")
			|| KeyView.IsDownNow("slash")
			|| "hint" == letterMouseDown)
			{
				model.hint();
			}
			SceneNodeView.SetVisible(view.hintButton, model.isHintVisible);

		}

		private void updateNewGame()
		{
			if (KeyView.IsDownNow("home")
			|| "new game" == letterMouseDown)
			{
				model.newGame();
			}
			SceneNodeView.SetVisible(view.newGameButton, model.isNewGameVisible);

		}

		private void updateContinue()
		{
			if (KeyView.IsDownNow("end")
			|| "continue" == letterMouseDown)
			{
				model.doContinue();
				resetSelect();
			}
			SceneNodeView.SetVisible(view.continueButton, model.isContinueVisible);

		}

		private void resetSelect()
		{
			int max = model.word.Count;
			for (int index = 0; index < max; index++)
			{
				string name = "bone_" + index + "/letter";
				AnimationView.SetState(
					SceneNodeView.GetChild(view.wordState, name), "none");
			}
		}

		/**
		 * Unity prohibits editing a property of position.
		 */
		private void updatePosition()
		{
			if (model.mayKnockback())
			{
				updateOutputHitsWord();
			}
			SceneNodeView.SetWorldY(view.word, model.wordPositionScaled);
			SceneNodeView.SetWorldY(view.progress, model.progressPositionTweened);
		}

		private void updateOutputHitsWord()
		{
			if (model.onOutputHitsWord())
			{
				AnimationView.SetState(view.wordState, "hit");
				view.audio.Play("explosion_big");
			}
		}
	}
}
