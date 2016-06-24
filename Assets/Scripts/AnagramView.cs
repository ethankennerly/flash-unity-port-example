using UnityEngine;
using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram 
{
	public sealed class AnagramView 
	{
		public List<SceneNode> letterNodes;
		internal AudioView audio;
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

		public void SetupAudio(List<string> soundFileNames)
		{
			audio = new AudioView();
			audio.Setup(SceneNodeView.GetName(main), soundFileNames, "Sounds/");
		}

		/**
		 * Cache found game objects.
		 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
		 */
		public void Setup(GameObject rootObject)
		{
			main = rootObject;
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

		internal void updateLetters(GameObject parent, List<string> letters, string namePattern, int letterMax) 
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
						var text3d = SceneNodeView.GetChild(letter, "text3d");
						TextView.SetText(text3d, letters[i]);
					}
				}
			}
		}
	}
}
