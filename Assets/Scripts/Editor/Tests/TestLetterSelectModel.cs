using NUnit.Framework;

namespace Finegamedesign.Utils
{
	public sealed class TestLetterSelectModel
	{
		[Test]
		public void PressLetterTwoTs()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.Populate("START");
			model.PressLetter("t");
			Assert.AreEqual(1, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			model.PressLetter("T");
			Assert.AreEqual(2, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			Assert.AreEqual(4, model.selectedIndexes.selects[1]);
		}
	}
}
