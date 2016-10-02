using NUnit.Framework;

namespace Finegamedesign.Utils
{
	public sealed class TestHint
	{
		[Test]
		public void GetText()
		{
			Hint hint = new Hint();
			hint.count = 2;
			Assert.AreEqual("HINT (2)", hint.GetText());
			hint.count = 1;
			Assert.AreEqual("HINT (1)", hint.GetText());
			hint.count = 0;
			Assert.AreEqual("GET HINTS", hint.GetText());
		}

		[Test]
		public void SelectFirst()
		{
			Hint hint = new Hint();
			hint.count = 1;
			hint.answer = "IDLE";
			hint.Select();
			Assert.AreEqual(0, hint.count);
			Assert.AreEqual(1, DataUtil.Length(hint.reveals));
			Assert.AreEqual("I", hint.reveals[0]);
		}

		[Test]
		public void SelectLastNoCharge()
		{
			Hint hint = new Hint();
			hint.count = 3;
			hint.answer = "ID";
			hint.Select();
			for (int request = 0; request < 4; request++)
			{
				hint.Select();
				Assert.AreEqual(1, hint.count);
				Assert.AreEqual(2, DataUtil.Length(hint.reveals));
				Assert.AreEqual("I", hint.reveals[0]);
				Assert.AreEqual("D", hint.reveals[1]);
			}
		}

		[Test]
		public void SelectNoWord()
		{
			Hint hint = new Hint();
			hint.count = 1;
			Assert.AreEqual(null, hint.answer);
			hint.Select();
			Assert.AreEqual(1, hint.count);
		}

		[Test]
		public void TODO_SaveAndLoad()
		{
		}
	}
}
