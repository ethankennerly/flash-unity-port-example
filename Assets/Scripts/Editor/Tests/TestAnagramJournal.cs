using NUnit.Framework;
using System.IO;

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
			Assert.AreEqual("newGame", model.journal.actionNow);
			Assert.AreEqual(50, model.journal.milliseconds);
			Assert.AreEqual(false, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual(null, model.journal.actionNow);
			model.Update(1.0f / 60.0f);
			model.Submit();
			Assert.AreEqual("wrong", model.state);
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual("submit", model.journal.actionNow);
			Assert.AreEqual(33, model.journal.milliseconds);
		}

		private void AssertPlaybackSubmitAndNewGame(AnagramModel model)
		{
			model.Setup();
			Assert.AreEqual(null, model.state);
			Assert.AreEqual(true, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual("newGame", model.journal.action);
			Assert.AreEqual("newGame", model.journal.actionNow);
			Assert.AreEqual("newGame", model.journal.commandNow);
			Assert.AreEqual(50, model.journal.milliseconds);
			Assert.AreEqual(false, model.isNewGameVisible);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual(null, model.journal.actionNow);
			model.Update(1.0f / 60.0f);
			Assert.AreEqual("wrong", model.state);
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual("submit", model.journal.actionNow);
			Assert.AreEqual("submit", model.journal.commandNow);
			Assert.AreEqual(33, model.journal.milliseconds);
		}

		[Test]
		public void RecordAndPlayback()
		{
			AnagramModel model = new AnagramModel();
			AssertRecordSubmitAndNewGame(model);
			model.journal.StartPlayback();
			AssertPlaybackSubmitAndNewGame(model);
			model.journal.isPlayback = false;
		}

		[Test]
		public void WriteAndRead()
		{
			AnagramModel model = new AnagramModel();
			AssertRecordSubmitAndNewGame(model);
			string historyTsv = "delay\taction\n50\tnewGame\n33\tsubmit";
			Assert.AreEqual(historyTsv, model.journal.Write());
			model.journal.ReadAndPlay(historyTsv);
			AssertPlaybackSubmitAndNewGame(model);
		}

		[Test]
		public void PlaybackActionEqualsCommand()
		{
			string historyTsv = File.ReadAllText("Assets/Scripts/test_journal.txt");
			AnagramModel model = new AnagramModel();
			model.Setup();
			model.journal.ReadAndPlay(historyTsv);
			while (model.journal.IsPlaying())
			{
				model.Update(1.0f / 60.0f);
				Assert.AreEqual(model.journal.commandNow,
					model.journal.actionNow);
			}
		}
	}
}
