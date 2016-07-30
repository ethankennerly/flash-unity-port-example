using UnityEngine;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramView : MonoBehaviour
	{
		// Larger number moves word more.
		public float wordPositionScale = 33.0f;
		public GameObject hint;

		public GameObject main;
		public GameObject word;
		public GameObject wordState;
		public GameObject input;
		public GameObject newGameButton;
		public GameObject continueButton;
		public GameObject inputState;
		public GameObject output;
		public GameObject deleteButton;
		public GameObject submitButton;
		public GameObject emailButton;
		public GameObject hintButton;
		public GameObject hud;
		public GameObject help;
		public GameObject helpText;
		public GameObject score;
		public GameObject progress;
		public GameObject level;
		public GameObject levelMax;

		internal List<SceneNodeModel> letterNodes;
		internal List<GameObject> wordBones;
		internal List<GameObject> wordLetters;
		internal TweenSwap tweenSwap = new TweenSwap();

		private AnagramController controller = new AnagramController();

		public void Start()
		{
			Setup(this.gameObject);
			controller.view = this;
			controller.Setup();
		}

		public void Update()
		{
			controller.Update(Time.deltaTime);
		}

		//
		// Cache found game objects.
		// http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
		// 
		internal void Setup(GameObject rootObject)
		{
			main = rootObject;
			word = SceneNodeView.GetChild(main, "canvas/word", word);
			wordState = SceneNodeView.GetChild(main, "canvas/word/state", wordState);
			input = SceneNodeView.GetChild(main, "input", input);
			inputState = SceneNodeView.GetChild(main, "input/state", inputState);
			output = SceneNodeView.GetChild(main, "input/output", output);
			hint = SceneNodeView.GetChild(main, "canvas/hints", hint);
			hintButton = SceneNodeView.GetChild(main, "canvas/hint", hintButton);
			newGameButton = SceneNodeView.GetChild(main, "canvas/newGame", newGameButton);
			continueButton = SceneNodeView.GetChild(main, "canvas/continue", continueButton);
			deleteButton = SceneNodeView.GetChild(main, "canvas/delete", deleteButton);
			submitButton = SceneNodeView.GetChild(main, "canvas/submit", submitButton);
			emailButton = SceneNodeView.GetChild(main, "canvas/email", emailButton);
			help = SceneNodeView.GetChild(main, "canvas/help", help);
			helpText = SceneNodeView.GetChild(main, "canvas/help/helpText", helpText);
			hud = SceneNodeView.GetChild(main, "canvas/hud", hud);
			score = SceneNodeView.GetChild(main, "canvas/hud/score", score);
			level = SceneNodeView.GetChild(main, "canvas/hud/level", level);
			levelMax = SceneNodeView.GetChild(main, "canvas/hud/levelMax", levelMax);
			progress = SceneNodeView.GetChild(main, "progress", progress);
			letterNodes = SceneNodeView.ToSceneNodeList(
				SceneNodeView.GetChildren(wordState));
		}

		internal List<GameObject> GetLetters(GameObject parent, string namePattern, int letterMax) 
		{
			List<GameObject> letters = new List<GameObject>();
			for (int i = 0; i < letterMax; i++)
			{
				string name = namePattern.Replace("{0}", i.ToString());
				var letter = SceneNodeView.GetChild(parent, name);
				letters.Add(letter);

			}
			return letters;
		}

		internal void UpdateLetters(GameObject parent, List<string> letters, string namePattern, int letterMax) 
		{
			for (int i = 0; i < letterMax; i++)
			{
				string name = namePattern.Replace("{0}", i.ToString());
				var letter = SceneNodeView.GetChild(parent, name);
				if (null != letter)
				{
					bool visible = i < letters.Count;
					SceneNodeView.SetVisible(letter, visible);
					if (visible) {
						var text = SceneNodeView.GetChild(letter, "text");
						TextView.SetText(text, letters[i]);
					}
				}
			}
		}
	}
}
