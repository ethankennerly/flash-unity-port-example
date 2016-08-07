using UnityEngine;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramView : MonoBehaviour
	{
		// Larger number moves word more.
		public bool isLogEnabled = true;
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

		public TextAsset journalHistoryTsv;

		public float timeScale = 1.0f;

		internal List<SceneNodeModel> letterNodes;
		internal List<GameObject> wordBones;
		internal List<GameObject> wordLetters;
		internal List<GameObject> inputLetters;
		internal List<GameObject> outputLetters;
		internal List<GameObject> hintLetters;

		internal TweenSwap tweenSwap = new TweenSwap();

		private AnagramController controller = new AnagramController();

		public void Start()
		{
			Toolkit.isLogEnabled = isLogEnabled;
			Setup(this.gameObject);
			controller.view = this;
			controller.Setup();
			if (null != journalHistoryTsv)
			{
				controller.model.journal.ReadAndPlay(journalHistoryTsv.text);
			}
		}

		public void Update()
		{
			Time.timeScale = timeScale;
			Toolkit.isLogEnabled = isLogEnabled;
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
			wordState = SceneNodeView.GetChild(word, "state", wordState);
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

		internal void UpdateLetters(List<GameObject> views, List<string> letters) 
		{
			for (int i = 0; i < DataUtil.Length(views); i++)
			{
				var letterView = views[i];
				if (null != letterView)
				{
					bool visible = i < letters.Count;
					SceneNodeView.SetVisible(letterView, visible);
					if (visible) {
						var textView = SceneNodeView.GetChild(letterView, "text");
						TextView.SetText(textView, letters[i]);
					}
				}
			}
		}
	}
}
