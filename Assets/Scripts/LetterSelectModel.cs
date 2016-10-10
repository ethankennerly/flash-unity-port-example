using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class LetterSelectModel
	{
		public bool isVerbose = false;
		public ToggleSuffix selectedIndexes = new ToggleSuffix();
		// Data type "char" would be more runtime-efficient in C#.
		// However "string" speeds up reusing algorithms.
		private List<string> word;
		private List<string> letters;
		
		public void PopulateWord(string text)
		{
			word = DataUtil.Split(text, "");
			letters = DataUtil.CloneList(word);
		}

		// Select first unselected letter.
		// Test case:  2016-10-02 Type second letter of a pair, 
		// such as word 1: "start" or word 506:  "teens".  
		// Expect select letter.  
		// Got some letters skipped.  Backspace.  Crash.
		// Example: Editor/Tests/TestLetterSelectModel.cs
		public bool Add(string letter)
		{
			letter = letter.ToUpper();
			int length = DataUtil.Length(selectedIndexes.selects);
			int selected = -1;
			if (length <= 0)
			{
				selected = letters.IndexOf(letter);
			}
			else
			{
				for (int s = 0; s < DataUtil.Length(letters); s++)
				{
					if (letter == letters[s])
					{
						if (selectedIndexes.selects.IndexOf(s) <= -1)
						{
							selected = s;
							break;
						}
					}
				}
			}
			if (isVerbose)
			{
				DebugUtil.Log("LetterSelectModel.Add: " + letter 
					+ " selected at " + selected);
			}
			bool isNow = 0 <= selected;
			if (isNow)
			{
				selectedIndexes.Add(selected);
			}
			return isNow;
		}

		// Return if in range.
		// Example: Editor/Tests/TestLetterSelectModel.cs
		public bool Toggle(int selected)
		{
			bool isInRange = 0 <= selected && selected < DataUtil.Length(word);
			if (isInRange)
			{
				selectedIndexes.Toggle(selected);
			}
			return isInRange;
		}

		// Return last selected letter if any to remove.
		// Example: Editor/Tests/TestLetterSelectModel.cs
		public string Pop()
		{
			string letter = null;
			
			if (1 <= DataUtil.Length(selectedIndexes.selects))
			{
				int index = DataUtil.Pop(selectedIndexes.selects);
				letter = letters[index];
			}
			return letter;
		}
	}
}
