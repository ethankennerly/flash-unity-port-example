using NUnit.Framework;

namespace Finegamedesign.Utils
{
	public sealed class TestToggleSuffix
	{
		[Test]
		public void Add()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(20);
			list.Toggle(40);
			Assert.AreEqual(2, DataUtil.Length(list.selected));
			Assert.AreEqual(20, list.selected[0]);
			Assert.AreEqual(40, list.selected[1]);
		}

		[Test]
		public void ToggleLast()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(20);
			list.Toggle(40);
			list.Toggle(10);
			list.Toggle(10);
			Assert.AreEqual(2, DataUtil.Length(list.selected));
			Assert.AreEqual(20, list.selected[0]);
			Assert.AreEqual(40, list.selected[1]);
		}

		[Test]
		public void ToggleFirst()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(40);
			list.Toggle(10);
			list.Toggle(20);
			list.Toggle(40);
			Assert.AreEqual(0, DataUtil.Length(list.selected));
		}

		[Test]
		public void ToggleMiddle()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(40);
			list.Toggle(20);
			list.Toggle(30);
			list.Toggle(70);
			list.Toggle(30);
			Assert.AreEqual(2, DataUtil.Length(list.selected));
			Assert.AreEqual(40, list.selected[0]);
			Assert.AreEqual(20, list.selected[1]);
		}
	}
}
