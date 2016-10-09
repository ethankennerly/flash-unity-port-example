using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class LetterSelectModel
	{
		public bool isVerbose = false;
		public ToggleSuffix selectedIndexes = new ToggleSuffix();
		private List<string> available;
		private List<string> selects;
		
		public void Populate(string text)
		{
			available = DataUtil.Split(text, "");
			selects = DataUtil.CloneList(available);
		}

		// Select first unselected letter.
		// Test case:  2016-10-02 Type second letter of a pair, 
		// such as word 1: "start" or word 506:  "teens".  
		// Expect select letter.  
		// Got some letters skipped.  Backspace.  Crash.
		// Example: Editor/Tests/TestAnagramModel.cs
		public void PressLetter(string letter)
		{
			letter = letter.ToUpper();
			int index = available.IndexOf(letter);
			int length = DataUtil.Length(selectedIndexes.selects);
			int selected = -1;
			if (length <= 0)
			{
				selected = selects.IndexOf(letter);
			}
			else
			{
				for (int s = 0; s < DataUtil.Length(selects); s++)
				{
					if (letter == selects[s])
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
				DebugUtil.Log("AnagramModel.PressLetter: " + letter 
					+ " available at " + index 
					+ " selected at " + selected);
			}
			if (0 <= index)
			{
				available.RemoveRange(index, 1);
				if (0 <= selected)
				{
					selectedIndexes.Add(selected);
					// Select(selected, letter);
					// inputs.Add(letter);
				}
			}
		}

	}
}
