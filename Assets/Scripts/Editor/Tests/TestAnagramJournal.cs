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
			AssertRecordSubmitAndNewGame(model);
		}

		private void AssertRecordSubmitAndNewGame(AnagramModel model)
		{
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
			string historyTsv = "delay\taction\n50\tnewGame\n33\tsubmit";
			model.journal.Read(historyTsv);
			AssertPlaybackSubmitAndNewGame(model);
		}

		private void AssertPlaybackSubmitAndNewGame(AnagramModel model)
		{
			model.Setup();
			model.journal.StartPlayback();
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

		[Test]
		public void RecordAndPlayback()
		{
			AnagramModel model = new AnagramModel();
			AssertRecordSubmitAndNewGame(model);
			AssertPlaybackSubmitAndNewGame(model);
			model.journal.isPlayback = false;
		}

	}
}
