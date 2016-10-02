using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class ToggleSuffix
	{
		public List<int> selected = new List<int>();

		public void Toggle(int itemIndex)
		{
			int indexInSelected = DataUtil.IndexOf(selected, itemIndex);
			if (0 <= indexInSelected)
			{
				DataUtil.Clear(selected, indexInSelected);
			}
			else
			{
				selected.Add(itemIndex);
			}
		}
	}
}
