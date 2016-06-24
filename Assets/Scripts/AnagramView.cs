using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramView 
	{
		public List<SceneNode> letterNodes;
		internal AudioView audio;
		internal Model model;
		internal Email email = new Email();
		internal string letterMouseDown;
		internal int letterIndexMouseDown;
		internal GameObject main;
		internal GameObject word;
		internal GameObject wordState;
		internal GameObject input;
		internal GameObject newGameButton;
		internal GameObject continueButton;
		internal GameObject inputState;
		internal GameObject output;
		internal GameObject hint;
		internal GameObject hintButton;
		internal GameObject hud;
		internal GameObject help;
		internal GameObject score;
		internal GameObject progress;
		internal GameObject level;
		internal GameObject levelMax;

		public void Setup(Model anagramModel, GameObject mainObject) 
		{
			model = anagramModel;
			main = mainObject;
			List<string> soundFileNames = new List<string>(){
				"select",
				"shoot",
				"explosion_big"
			};
			audio = new AudioView();
			audio.Setup(SceneNodeView.GetName(main), soundFileNames, "Sounds/");
			word = SceneNodeView.GetChild(main, "word");
			wordState = SceneNodeView.GetChild(main, "word/state");
			input = SceneNodeView.GetChild(main, "input");
			inputState = SceneNodeView.GetChild(main, "input/state");
			output = SceneNodeView.GetChild(main, "input/output");
			hint = SceneNodeView.GetChild(main, "input/hints");
			hintButton = SceneNodeView.GetChild(main, "input/hint");
			newGameButton = SceneNodeView.GetChild(main, "input/newGame");
			continueButton = SceneNodeView.GetChild(main, "input/continue");
			hud = SceneNodeView.GetChild(main, "canvas/hud");
			help = SceneNodeView.GetChild(main, "canvas/help");
			score = SceneNodeView.GetChild(main, "canvas/hud/score");
			progress = SceneNodeView.GetChild(main, "progress");
			level = SceneNodeView.GetChild(main, "canvas/hud/level");
			levelMax = SceneNodeView.GetChild(main, "canvas/hud/levelMax");
			letterNodes = SceneNodeView.ToSceneNodeList(
				SceneNodeView.GetChildren(wordState));
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
				GameObject text3d = SceneNodeView.GetChild(MouseView.target, "text3d");
				letterMouseDown = TextView.GetText(text3d).ToLower();
				string name = SceneNodeView.GetName(
					SceneNodeView.GetParent(MouseView.target));
				letterIndexMouseDown = Toolkit.ParseIndex(name);
				// Debug.Log("View.updateMouseDown: " + letterMouseDown);
			}
		}

		public bool IsLetterKeyDown(string letter)
		{
			return Input.GetKeyDown(letter.ToLower());
		}

		public void update()
		{
			updateCheat();
			updateMouseDown();
			updateBackspace();
			if (model.isGamePlaying) {
				updatePlay();
			}
			updateLetters(wordState, model.word, "bone_{0}/letter");
			updateLetters(inputState, model.inputs, "Letter_{0}");
			updateLetters(output, model.outputs, "Letter_{0}");
			updateLetters(hint, model.hints, "Letter_{0}");
			updateHud();
			updateHint();
			updateContinue();
			updateNewGame();
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
			if (Input.GetKeyDown("page up"))
			{
				model.levelUp();
				audio.Play("select");
			}
			else if (Input.GetKeyDown("page down"))
			{
				model.levelDownMax();
				audio.Play("select");
			}
			else if (Input.GetKeyDown("0")
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
			if (Input.GetKeyDown("delete")
			|| Input.GetKeyDown("backspace")
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
				AnimationView.SetState(SceneNodeView.GetChild(wordState, name), state);
				audio.Play("select");
			}
		}

		private void updateHud()
		{
			SceneNodeView.SetVisible(hud, model.isHudVisible);
			TextView.SetText(help, model.help);
			TextView.SetText(score, model.score.ToString());
			TextView.SetText(level, model.progress.level.ToString());
			TextView.SetText(levelMax, model.progress.levelMax.ToString());
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
			if (Input.GetKeyDown("space")
			|| Input.GetKeyDown("return")
			|| "submit" == letterMouseDown)
			{
				letterMouseDown = null;
				string state = model.submit();
				if (null != state) 
				{
					// TODO AnimationView.SetState(wordState, state);
					AnimationView.SetState(input, state);
					audio.Play("shoot");
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
			if (Input.GetKeyDown(KeyCode.Question)
			|| Input.GetKeyDown(KeyCode.Slash)
			|| "hint" == letterMouseDown)
			{
				model.hint();
			}
			SceneNodeView.SetVisible(hintButton, model.isHintVisible);

		}

		private void updateNewGame()
		{
			if (Input.GetKeyDown(KeyCode.Home)
			|| "new game" == letterMouseDown)
			{
				model.newGame();
			}
			SceneNodeView.SetVisible(newGameButton, model.isNewGameVisible);

		}

		private void updateContinue()
		{
			if (Input.GetKeyDown(KeyCode.End)
			|| "continue" == letterMouseDown)
			{
				model.doContinue();
				resetSelect();
			}
			SceneNodeView.SetVisible(continueButton, model.isContinueVisible);

		}

		private void resetSelect()
		{
			int max = model.word.Count;
			for (int index = 0; index < max; index++)
			{
				string name = "bone_" + index + "/letter";
				AnimationView.SetState(SceneNodeView.GetChild(wordState, name), "none");
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
			SceneNodeView.SetWorldY(word, model.wordPositionScaled);
			SceneNodeView.SetWorldY(progress, model.progressPositionTweened);
		}

		private void updateOutputHitsWord()
		{
			if (model.onOutputHitsWord())
			{
				AnimationView.SetState(wordState, "hit");
				audio.Play("explosion_big");
			}
		}

		/**
		 * Find could be cached.
		 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
		 */
		internal void updateLetters(GameObject parent, List<string> letters, string namePattern) 
		{
			int max = model.letterMax;
			for (int i = 0; i < max; i++)
			{
				string name = namePattern.Replace("{0}", i.ToString());
				GameObject letter = SceneNodeView.GetChild(parent, name);
				if (null != letter)
				{
					bool visible = i < letters.Count;
					SceneNodeView.SetVisible(letter, visible);
					if (visible) {
						GameObject text3d = SceneNodeView.GetChild(letter, "text3d");
						TextView.SetText(text3d, letters[i]);
					}
				}
			}
		}
	}
}
