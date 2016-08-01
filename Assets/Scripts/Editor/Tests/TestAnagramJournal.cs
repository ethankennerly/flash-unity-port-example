using NUnit.Framework;
using System.IO;
using System.Linq/*<Sum>*/;
using UnityEngine/*<Mathf>*/;

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
			AnagramModel model = new AnagramModel();
			Words.Setup(model);
			AssertPlaybackActionEqualsCommand(model,
				"Assets/Scripts/Editor/Tests/test_journal.txt");
			model = new AnagramModel();
			Words.Setup(model);
			AssertPlaybackActionEqualsCommand(model,
				"Assets/Scripts/Editor/Tests/ethan_20160731.txt");
		}

		private void AssertPlaybackActionEqualsCommand(
				AnagramModel model, string filePath)
		{
			string historyTsv = File.ReadAllText(filePath);
			model.Setup();
			model.journal.ReadAndPlay(historyTsv);
			float time = 0.0f;
			float step = 1.0f / 60.0f;
			float recorded = (float)(
				model.journal.playbackDelays.Sum()) / 1000.0f;
			string message = "file " + filePath;
			while (model.journal.IsPlaying())
			{
				time += step;
				model.Update(step);
				message = "file " + filePath
					+ " playback index " + model.journal.playbackIndex
					+ " word " + model.text;
				Assert.AreEqual(model.journal.commandNow,
					model.journal.actionNow, message);
			}
			float difference = Mathf.Abs(time - recorded);
			message = "file " + filePath
				+ " playback " + time.ToString() 
				+ " record " + recorded.ToString() 
				+ " difference " + difference;
			Assert.AreEqual(true, difference < 0.01f, message);
		}
	}
}
