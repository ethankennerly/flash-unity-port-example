using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public class AnagramView 
	{
		public List<SceneNode> letterNodes;
		private AudioView audio;
		private Model model;
		private Email email = new Email();
		private string letterMouseDown;
		private int letterIndexMouseDown;
		private GameObject main;
		private GameObject prompt;
		private GameObject input;
		private GameObject output;
		private GameObject hint;
		private GameObject hud;
		private GameObject help;
		private GameObject score;
		private GameObject level;
		private GameObject levelMax;

		public AnagramView(Model theModel, Main theMainScene) 
		{
			model = theModel;
			main = theMainScene.gameObject;
			string[] soundFileNames = new string[]{
				"select",
				"shoot",
				"explosion_big"
			};
			audio = new AudioView();
			audio.Setup(theMainScene.name, soundFileNames, "Sounds/");
			prompt = SceneNodeView.GetChild(main, "word/state");
			input = SceneNodeView.GetChild(main, "input/state");
			output = SceneNodeView.GetChild(main, "input/output");
			hint = SceneNodeView.GetChild(main, "input/hints");
			hud = SceneNodeView.GetChild(main, "canvas/hud");
			help = SceneNodeView.GetChild(main, "canvas/help");
			score = SceneNodeView.GetChild(main, "canvas/hud/score");
			level = SceneNodeView.GetChild(main, "canvas/hud/level");
			levelMax = SceneNodeView.GetChild(main, "canvas/hud/levelMax");
			letterNodes = SceneNodeView.ToSceneNodeList(
				SceneNodeView.GetChildren(prompt));
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
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					Transform transform = hit.transform.Find("text3d");
					letterMouseDown = transform.GetComponent<TextMesh>().text.ToLower();
					string name = transform.parent.parent.gameObject.name;
					letterIndexMouseDown = Toolkit.ParseIndex(name);
					// Debug.Log("View.updateMouseDown: " + letterMouseDown);
				}
			}
		}

		private bool IsLetterMouseDown(string letter)
		{
			return letter == letterMouseDown;
		}

		public bool IsLetterKeyDown(string letter)
		{
			return Input.GetKeyDown(letter.ToLower());
				// || IsLetterMouseDown(letter.ToLower());
		}

		public void update()
		{
			updateCheat();
			updateMouseDown();
			updateBackspace();
			if (model.isGamePlaying) {
				updatePlay();
			}
			updateLetters(prompt, model.word, "bone_{0}/letter");
			updateLetters(input, model.inputs, "Letter_{0}");
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
				AnimationView.SetState(prompt.transform.Find(name).gameObject, state);
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
					// TODO main.word.gotoAndPlay(state);
					AnimationView.SetState(main.transform.Find("input").gameObject, state);
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
			SceneNodeView.SetVisible(main.transform.Find("input/hint").gameObject, model.isHintVisible);

		}

		private void updateNewGame()
		{
			if (Input.GetKeyDown(KeyCode.Home)
			|| "new game" == letterMouseDown)
			{
				model.newGame();
			}
			SceneNodeView.SetVisible(main.transform.Find("input/newGame").gameObject, model.isNewGameVisible);

		}

		private void updateContinue()
		{
			if (Input.GetKeyDown(KeyCode.End)
			|| "continue" == letterMouseDown)
			{
				model.doContinue();
				resetSelect();
			}
			SceneNodeView.SetVisible(main.transform.Find("input/continue").gameObject, model.isContinueVisible);

		}

		private void resetSelect()
		{
			GameObject parent = main.transform.Find("word").gameObject.transform.Find("state").gameObject;
			int max = model.word.Count;
			for (int index = 0; index < max; index++)
			{
				string name = "bone_" + index + "/letter";
				AnimationView.SetState(parent.transform.Find(name).gameObject, "none");
			}
		}

		/**
		 * Unity prohibits editing a property of position.
		 */
		private void setPositionY(Transform transform, float y)
		{
			Vector3 position = transform.position;
			position.y = y;
			transform.position = position;
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
			setPositionY(main.transform.Find("word"), model.wordPositionScaled);
			setPositionY(main.transform.Find("progress"), model.progressPositionTweened);
		}

		private void updateOutputHitsWord()
		{
			if (model.onOutputHitsWord())
			{
				AnimationView.SetState(prompt, "hit");
				audio.Play("explosion_big");
			}
		}

		/**
		 * Find could be cached.
		 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
		 */
		private void updateLetters(GameObject parent, List<string> letters, string namePattern) 
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
						TextMesh mesh = text3d.GetComponent<TextMesh>();
						mesh.text = letters[i];
					}
				}
			}
		}
	}
}
