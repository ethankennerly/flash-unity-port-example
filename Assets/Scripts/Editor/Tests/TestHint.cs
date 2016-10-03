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
		public void StorePurchaseAndClose()
		{
			Hint hint = new Hint();
			hint.count = 1;
			hint.cents = 200;
			hint.countCents[0][0] = 10;
			hint.countCents[0][1] = 99;
			hint.countCents[1][0] = 45;
			hint.countCents[1][1] = 399;
			Assert.AreEqual(null, hint.state);
			hint.Store();
			Assert.AreEqual("store", hint.state);
			Assert.AreEqual("10 HINTS", hint.GetCountText(0));
			Assert.AreEqual("0.99 USD", hint.GetPriceText(0));
			Assert.AreEqual("45 HINTS", hint.GetCountText(1));
			Assert.AreEqual("3.99 USD", hint.GetPriceText(1));
			hint.Purchase(0);
			Assert.AreEqual(11, hint.count);
			Assert.AreEqual(101, hint.cents);
			Assert.AreEqual("purchased", hint.state);
			hint.Close();
			Assert.AreEqual("close", hint.state);
		}

		[Test]
		public void TODO_SaveAndLoad()
		{
		}
	}
}
