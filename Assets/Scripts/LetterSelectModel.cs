using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class LetterSelectModel
	{
		public bool isVerbose = false;
		public List<string> inputs = new List<string>();
		public ToggleSuffix selectedIndexes = new ToggleSuffix();
		// Data type "char" would be more runtime-efficient in C#.
		// However "string" speeds up reusing algorithms.
		public List<string> letters;
		public List<string> word;
		
		public void PopulateWord(string text)
		{
			word = DataUtil.Split(text, "");
			letters = DataUtil.CloneList(word);
			DataUtil.Clear(inputs);
			DataUtil.Clear(selectedIndexes.selects);
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
				inputs.Add(letters[selected]);
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
				DataUtil.Clear(inputs);
				int length = DataUtil.Length(selectedIndexes.selects);
				for (int index = 0; index < length; index++)
				{
					int selectedIndex = selectedIndexes.selects[index];
					inputs.Add(letters[selectedIndex]);
				}
			}
			return isInRange;
		}

		// Return last selected letter if any to remove.
		// Example: Editor/Tests/TestLetterSelectModel.cs
		public string Pop()
		{
			string letter = null;
			int index = selectedIndexes.Pop();
			if (0 <= index)
			{
				letter = letters[index];
				DataUtil.Pop(inputs);
			}
			return letter;
		}
	}
}
