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
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(20, list.selects[0]);
			Assert.AreEqual(40, list.selects[1]);
			Assert.AreEqual(0, DataUtil.Length(list.deselectsNow));
			Assert.AreEqual(1, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(40, list.selectsNow[0]);
		}

		[Test]
		public void ToggleLast()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(20);
			list.Toggle(40);
			list.Toggle(10);
			list.Toggle(10);
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(20, list.selects[0]);
			Assert.AreEqual(40, list.selects[1]);
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(1, DataUtil.Length(list.deselectsNow));
			Assert.AreEqual(10, list.deselectsNow[0]);
		}

		[Test]
		public void ToggleFirst()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(40);
			list.Toggle(10);
			list.Toggle(20);
			list.Toggle(40);
			Assert.AreEqual(0, DataUtil.Length(list.selects));
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(3, DataUtil.Length(list.deselectsNow));
			Assert.AreEqual(40, list.deselectsNow[0]);
			Assert.AreEqual(10, list.deselectsNow[1]);
			Assert.AreEqual(20, list.deselectsNow[2]);
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
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(40, list.selects[0]);
			Assert.AreEqual(20, list.selects[1]);
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(2, DataUtil.Length(list.deselectsNow));
			Assert.AreEqual(30, list.deselectsNow[0]);
			Assert.AreEqual(70, list.deselectsNow[1]);
		}
	}
}
