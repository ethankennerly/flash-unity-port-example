using NUnit.Framework;

namespace Finegamedesign.Utils
{
	public sealed class TestLetterSelectModel
	{
		[Test]
		public void PopulateWordNothingSelected()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.PopulateWord("START");
			Assert.AreEqual(true, model.Add("t"));
			model.PopulateWord("SPELL");
			Assert.AreEqual(0, DataUtil.Length(model.selectedIndexes.selects));
		}

		[Test]
		public void AddTwoTs()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.PopulateWord("START");
			Assert.AreEqual(true, model.Add("t"));
			Assert.AreEqual(1, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			Assert.AreEqual(1, DataUtil.Length(model.inputs));
			Assert.AreEqual("T", model.inputs[0]);
			Assert.AreEqual(true, model.Add("T"));
			Assert.AreEqual(2, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			Assert.AreEqual(4, model.selectedIndexes.selects[1]);
			Assert.AreEqual(2, DataUtil.Length(model.inputs));
			Assert.AreEqual("T", model.inputs[0]);
			Assert.AreEqual("T", model.inputs[1]);
			Assert.AreEqual(false, model.Add("T"));
			Assert.AreEqual(2, DataUtil.Length(model.selectedIndexes.selects));
		}

		[Test]
		public void AddIgnoredLetter()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.PopulateWord("START");
			Assert.AreEqual(false, model.Add("Q"));
			Assert.AreEqual(0, DataUtil.Length(model.selectedIndexes.selects));
		}

		[Test]
		public void ToggleSuffix()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.PopulateWord("START");
			Assert.AreEqual(false, model.Toggle(-1));
			Assert.AreEqual(true, model.Toggle(1));
			Assert.AreEqual(1, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			Assert.AreEqual(1, DataUtil.Length(model.inputs));
			Assert.AreEqual("T", model.inputs[0]);
			Assert.AreEqual(true, model.Toggle(4));
			Assert.AreEqual(2, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(1, model.selectedIndexes.selects[0]);
			Assert.AreEqual(4, model.selectedIndexes.selects[1]);
			Assert.AreEqual(2, DataUtil.Length(model.inputs));
			Assert.AreEqual("T", model.inputs[0]);
			Assert.AreEqual("T", model.inputs[1]);
			Assert.AreEqual(true, model.Toggle(1));
			Assert.AreEqual(0, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(0, DataUtil.Length(model.inputs));
		}

		[Test]
		public void PopOne()
		{
			LetterSelectModel model = new LetterSelectModel();
			model.PopulateWord("START");
			Assert.AreEqual(true, model.Add("t"));
			Assert.AreEqual("T", model.Pop());
			Assert.AreEqual(0, DataUtil.Length(model.selectedIndexes.selects));
			Assert.AreEqual(null, model.Pop());
			Assert.AreEqual(0, DataUtil.Length(model.inputs));
		}
	}
}
