using System.Collections.Generic;

namespace Finegamedesign.Utils
{
	public sealed class ToggleSuffix
	{
		public List<int> selects = new List<int>();
		// When only one can be selected between updates,
		// then this can be simplified to a single integer.
		public List<int> selectsNow = new List<int>();
		public List<int> removesNow = new List<int>();
		public int selectNow = -1;
		public int removeNow = -1;

		public void Add(int itemIndex)
		{
			selects.Add(itemIndex);
			selectsNow.Add(itemIndex);
			selectNow = itemIndex;
		}

		public void Toggle(int itemIndex)
		{
			int indexInSelected = DataUtil.IndexOf(selects, itemIndex);
			if (0 <= indexInSelected)
			{
				for (int index = indexInSelected; index < DataUtil.Length(selects); index++)
				{
					int item = selects[index];
					removesNow.Add(item);
					removeNow = item;
				}
				DataUtil.Clear(selects, indexInSelected);
			}
			else
			{
				Add(itemIndex);
			}
		}

		public int Pop()
		{
			int length = DataUtil.Length(selects);
			if (length <= 0)
			{
				return -1;
			}
			int item = DataUtil.Pop(selects);
			removesNow.Add(item);
			removeNow = item;
			return item;
		}

		// Each update clears items selected now and removed now.
		public void Update()
		{
			DataUtil.Clear(selectsNow);
			DataUtil.Clear(removesNow);
			removeNow = -1;
			selectNow = -1;
		}
	}
}
