using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class ToggleSuffix
	{
		public List<int> selects = new List<int>();
		public List<int> selectsNow = new List<int>();
		public List<int> removesNow = new List<int>();

		public void Toggle(int itemIndex)
		{
			int indexInSelected = DataUtil.IndexOf(selects, itemIndex);
			DataUtil.Clear(selectsNow);
			DataUtil.Clear(removesNow);
			if (0 <= indexInSelected)
			{
				for (int index = indexInSelected; index < DataUtil.Length(selects); index++)
				{
					int item = selects[index];
					removesNow.Add(item);
				}
				DataUtil.Clear(selects, indexInSelected);
			}
			else
			{
				selects.Add(itemIndex);
				selectsNow.Add(itemIndex);
			}
		}
	}
}