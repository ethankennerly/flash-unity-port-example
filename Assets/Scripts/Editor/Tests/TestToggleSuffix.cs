using NUnit.Framework;

namespace Finegamedesign.Utils
{
	public sealed class TestToggleSuffix
	{
		private static void AssertUpdateClear(ToggleSuffix list)
		{
			list.Update();
			Assert.AreEqual(0, DataUtil.Length(list.removesNow), 
				"Expected update clears removes now");
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow),
				"Expected update clears selects now");
		}

		public ToggleSuffix Add()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(20);
			list.Update();
			list.Toggle(40);
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(20, list.selects[0]);
			Assert.AreEqual(40, list.selects[1]);
			return list;
		}

		[Test]
		public void AddUpdate()
		{
			ToggleSuffix list = Add();
			Assert.AreEqual(0, DataUtil.Length(list.removesNow));
			Assert.AreEqual(1, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(40, list.selectsNow[0]);
			AssertUpdateClear(list);
		}

		public ToggleSuffix ToggleLast()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(20);
			list.Toggle(40);
			list.Toggle(10);
			list.Update();
			list.Toggle(10);
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(20, list.selects[0]);
			Assert.AreEqual(40, list.selects[1]);
			return list;
		}

		[Test]
		public void ToggleLastUpdate()
		{
			ToggleSuffix list = ToggleLast();
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(1, DataUtil.Length(list.removesNow));
			Assert.AreEqual(10, list.removesNow[0]);
			AssertUpdateClear(list);
		}

		public ToggleSuffix ToggleFirst()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(40);
			list.Toggle(10);
			list.Toggle(20);
			list.Update();
			list.Toggle(40);
			Assert.AreEqual(0, DataUtil.Length(list.selects));
			return list;
		}

		[Test]
		public void ToggleFirstUpdate()
		{
			ToggleSuffix list = ToggleFirst();
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(3, DataUtil.Length(list.removesNow));
			Assert.AreEqual(40, list.removesNow[0]);
			Assert.AreEqual(10, list.removesNow[1]);
			Assert.AreEqual(20, list.removesNow[2]);
		}

		public ToggleSuffix ToggleMiddle()
		{
			ToggleSuffix list = new ToggleSuffix();
			list.Toggle(40);
			list.Toggle(20);
			list.Toggle(30);
			list.Toggle(70);
			list.Update();
			list.Toggle(30);
			Assert.AreEqual(2, DataUtil.Length(list.selects));
			Assert.AreEqual(40, list.selects[0]);
			Assert.AreEqual(20, list.selects[1]);
			return list;
		}

		[Test]
		public void ToggleMiddleUpdate()
		{
			ToggleSuffix list = ToggleMiddle();
			Assert.AreEqual(0, DataUtil.Length(list.selectsNow));
			Assert.AreEqual(2, DataUtil.Length(list.removesNow));
			Assert.AreEqual(30, list.removesNow[0]);
			Assert.AreEqual(70, list.removesNow[1]);
			AssertUpdateClear(list);
		}
	}
}
