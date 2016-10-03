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
		public int cents = 0;
		public int count = 0;
		public string answer = null;
		public string state = null;
		public List<string> reveals = new List<string>();
		// After WordBrain Themes.
		public List<List<int>> countCents = new List<List<int>>(){
			new List<int>(){10, 99},
			new List<int>(){45, 399},
			new List<int>(){125, 999},
			new List<int>(){200, 1499}
		};
		private int countIndex = 0;
		private int centsIndex = 1;
		private int centsPerDollar = 100;

		// Example: Editor/Tests/TestHint.cs
		public string GetCountText(int productIndex)
		{
			return countCents[productIndex][countIndex] + " HINTS";
		}

		// Example: Editor/Tests/TestHint.cs
		public string GetPriceText(int productIndex)
		{
			int cents = countCents[productIndex][centsIndex];
			int dollars = cents / centsPerDollar;
			int remainder = cents % centsPerDollar;
			return dollars + "." + remainder + " USD";
		}

		// Example: Editor/Tests/TestHint.cs
		public void Store()
		{
			state = "store";
		}

		public void Purchase(int productIndex)
		{
			state = "purchased";
			var product = countCents[productIndex];
			count += product[countIndex];
			cents -= product[centsIndex];
		}

		public void Close()
		{
			state = "close";
		}

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

		// Example: Editor/Tests/TestHint.cs
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
