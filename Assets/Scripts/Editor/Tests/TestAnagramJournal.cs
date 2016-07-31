using NUnit.Framework;

namespace Finegamedesign.Anagram
{
	[TestFixture]
	public sealed class TestAnagramJournal
	{
		[Test]
		public void RecordSubmitAndNewGame()
		{
			AnagramModel model = new AnagramModel();
			model.Setup();
			Assert.AreEqual(null, model.state);
			Assert.AreEqual(true, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.NewGame();
			Assert.AreEqual("newGame", model.journal.action);
			Assert.AreEqual(50, model.journal.milliseconds);
			Assert.AreEqual(false, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Submit();
			Assert.AreEqual("wrong", model.state);
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual(33, model.journal.milliseconds);
		}

		[Test]
		public void PlaybackSubmitAndNewGame()
		{
			AnagramModel model = new AnagramModel();
			model.Setup();
			string historyTsv = "delay\taction\n50\tnewGame\n33\tsubmit";
			model.journal.Read(historyTsv);
			model.journal.isPlayback = true;
			Assert.AreEqual(null, model.state);
			Assert.AreEqual(true, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual("newGame", model.journal.action);
			Assert.AreEqual(50, model.journal.milliseconds);
			Assert.AreEqual(false, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual("wrong", model.state);
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual(33, model.journal.milliseconds);
		}
	}
}
