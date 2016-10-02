using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	// Features:
	//	Read hints remaining.
	// TODO:
	//	Hint button always available.  Costs 1 hint.
	//	Save and load hints.
	//	If tap hint button with no hints, or from start/pause screen button: Get more hints.
	//	Hints on random letter of some milestone words.  Example:  WordBrain Themes.
	//		Spell with the letter to get a hint.
	//	Progress to a hint on some milestone words.  Example:  WordBrain Themes.
	public sealed class Hint
	{
		public int count = 0;
		public string answer = null;
		public List<string> reveals = new List<string>();


		// Example: Editor/Tests/TestHint.cs
		public string GetText()
		{
			string text;
			if (1 <= count)
			{
				text = "HINT (" + count + ")";
			}
			else
			{
				text = "GET HINTS";
			}
			return text;
		}

		public void Select()
		{
			if (1 <= count && null != answer)
			{
				int revealsLength = DataUtil.Length(reveals);
				int answerLength = DataUtil.Length(answer);
				if (revealsLength < answerLength)
				{
					count--;
					string letter = answer.Substring(revealsLength, 1);
					reveals.Add(letter);
				}
			}
		}
	}
}
