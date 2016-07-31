using NUnit.Framework;

namespace Finegamedesign.Anagram
{
	[TestFixture]
	public sealed class TestAnagramJournal
	{
		[Test]
		public void SubmitAndNewGame()
		{
			AnagramModel model = new AnagramModel();
			model.Setup();
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Submit();
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual(33, model.journal.milliseconds);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.NewGame();
			Assert.AreEqual("newGame", model.journal.action);
			Assert.AreEqual(50, model.journal.milliseconds);
		}
	}
}
