using NUnit.Framework;
using Finegamedesign.Anagram;
using System.Collections.Generic;

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
			Assert.AreEqual(true, hint.Select());
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
			Assert.AreEqual(true, hint.Select());
			Assert.AreEqual(true, hint.Select());
			for (int request = 0; request < 4; request++)
			{
				Assert.AreEqual(false, hint.Select());
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
			Assert.AreEqual(false, hint.Select());
			Assert.AreEqual(1, hint.count);
		}

		[Test]
		public void SelectNotVisible()
		{
			Hint hint = new Hint();
			hint.count = 1;
			hint.answer = "BE";
			hint.isVisible = false;
			Assert.AreEqual(false, hint.Select());
			Assert.AreEqual(1, hint.count);
			Assert.AreEqual("none", hint.state);
		}

		[Test]
		public void SelectCountZeroGotoStore()
		{
			Hint hint = new Hint();
			hint.count = 0;
			hint.answer = "IDLE";
			Assert.AreEqual(false, hint.Select());
			Assert.AreEqual(0, hint.count);
			Assert.AreEqual("store", hint.state);
		}

		[Test]
		public void Load()
		{
			Hint hint = new Hint();
			hint.Load(null);
			Assert.AreEqual(0, hint.count);
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["hint"] = 20;
			Assert.AreEqual(0, hint.count);
			hint.Load(data);
			Assert.AreEqual(20, hint.count);
			data["cents"] = 99;
			Assert.AreEqual(0, hint.cents);
			hint.Load(data);
			Assert.AreEqual(99, hint.cents);
		}

		[Test]
		public void StoreAndClose()
		{
			Hint hint = new Hint();
			Assert.AreEqual("none", hint.state);
			hint.Store();
			Assert.AreEqual("store", hint.state);
			hint.Close();
			Assert.AreEqual("close", hint.state);
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
			Assert.AreEqual("none", hint.state);
			hint.Store();
			Assert.AreEqual("store", hint.state);
			Assert.AreEqual("10 HINTS", hint.GetCountText(0));
			Assert.AreEqual("0.99 USD", hint.GetPriceText(0));
			Assert.AreEqual("45 HINTS", hint.GetCountText(1));
			Assert.AreEqual("3.99 USD", hint.GetPriceText(1));
			hint.Purchase(0);
			Assert.AreEqual(11, hint.count);
			Assert.AreEqual(101, hint.cents);
			Assert.AreEqual("close", hint.state);
			hint.Close();
			Assert.AreEqual("close", hint.state);
		}

		[Test]
		public void AnagramControllerSaveAndLoad()
		{
			AnagramController controller = new AnagramController();
			controller.storage.name = "test_hint";
			controller.model.hint.count = 11;
			controller.model.hint.cents = 101;
			controller.Save();
			controller = new AnagramController();
			controller.storage.name = "test_hint";
			controller.Setup();
			Assert.AreEqual(11, controller.model.hint.count);
			Assert.AreEqual(101, controller.model.hint.cents);
		}
	}
}
