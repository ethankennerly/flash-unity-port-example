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
			Assert.AreEqual("HINT (0)", hint.GetText());
		}

		[Test]
		public void TODO_SaveAndLoad()
		{
		}
	}
}
