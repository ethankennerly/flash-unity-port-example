using NUnit.Framework/*<Assert, Test>*/;
using Finegamedesign.Utils;
using System.Collections.Generic;

namespace Finegamedesign.Anagram
{
	public sealed class TestAnagramModel
	{
		private static string DescribeTrialCycle(AnagramModel model)
		{
			return "trial count " + model.trialCount
				+ " in trial period " + model.trialPeriod;
		}

		[Test]
		public void IsTrialCycleNow()
		{
			AnagramModel model = new AnagramModel();
			model.progress.level = model.tutorLevel;
			Assert.AreEqual(false, model.IsTutor(), "tutor");
			model.trialPeriod = 10;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 11;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 19;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 20;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 21;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
		}

		[Test]
		public void IsTrialCycleNowTutorial()
		{
			AnagramModel model = new AnagramModel();
			Assert.AreEqual(true, model.IsTutor(), "tutor");
			model.trialPeriod = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 4;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 5;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
		}

		[Test]
		public void UpdateTrialCycleCheckpoint()
		{
			AnagramModel model = new AnagramModel();
			model.progress.level = model.tutorLevel;
			Assert.AreEqual(false, model.IsTutor(), "tutor");
			model.trialPeriod = 3;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 0;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 1;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 2;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 3;
			Assert.AreEqual(true, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(true, model.isContinueVisible, "Is continue visible");
			model.isContinueVisible = false;
			model.trialCount = 4;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
		}

		[Test]
		public void UpdateHelpStateNowRepeat()
		{
			AnagramModel model = new AnagramModel();
			model.Update(0.0f);
			Assert.AreEqual(null, model.helpStateNow);
			model.helpTextNow.next = "WORD REPEATED";
			model.helpState.next = "repeat";
			model.Update(0.0f);
			Assert.AreEqual("beginNow", model.helpStateNow);
			model.Update(0.0f);
			Assert.AreEqual(null, model.helpStateNow);
			model.helpTextNow.next = "";
			model.helpState.next = "";
			model.Update(0.0f);
			Assert.AreEqual("endNow", model.helpStateNow);
			model.Update(0.0f);
			Assert.AreEqual(null, model.helpStateNow);
		}

		[Test]
		public void IsHelpRepeatWhenHelpTextNull()
		{
			AnagramModel model = new AnagramModel();
			model.helpTextNow.next = null;
			Assert.AreEqual(true, model.IsHelpRepeat());
			model.progress.level = 100;
			Assert.AreEqual(false, model.IsHelpRepeat());
			model.progress.level = 10;
			Assert.AreEqual(true, model.IsHelpRepeat());
		}

		[Test]
		public void UpdateHelpStateNowEndOnce()
		{
			AnagramModel model = new AnagramModel();
			model.helpTextNow.next = "WORD REPEATED";
			model.helpState.next = "repeat";
			model.Update(0.0f);
			model.helpTextNow.next = "";
			model.helpState.next = "";
			model.Update(0.0f);
			Assert.AreEqual("endNow", model.helpStateNow);
			model.helpTextNow.next = "";
			model.helpState.next = "none";
			model.Update(0.0f);
			Assert.AreEqual("endNow", model.helpStateNow, "state from '' to 'none'");
		}

		[Test]
		public void IsHelpByState()
		{
			Assert.AreEqual(false, AnagramModel.IsHelpByState("none", false, true));
			Assert.AreEqual(true, AnagramModel.IsHelpByState("none", true, true));
			Assert.AreEqual(false, AnagramModel.IsHelpByState("none", true, false));
			Assert.AreEqual(false, AnagramModel.IsHelpByState("help", false, true));
			Assert.AreEqual(false, AnagramModel.IsHelpByState("help", true, true));
			Assert.AreEqual(false, AnagramModel.IsHelpByState("help", true, false));
			Assert.AreEqual(true, AnagramModel.IsHelpByState("", true, true));
			Assert.AreEqual(true, AnagramModel.IsHelpByState(null, true, true));
			Assert.AreEqual(false, AnagramModel.IsHelpByState(null, false, true));
			Assert.AreEqual(true, AnagramModel.IsHelpByState("endNow", true, true));
		}

		[Test]
		public void UpdateIsHelpByPosition()
		{
			AnagramModel model = new AnagramModel();
			model.helpLevel = 100;
			model.helpPosition = 0.9f;
			model.width = 720.0f;
			model.Setup();
			model.progress.level = model.tutorLevel + 1;
			var parameters = new Dictionary<string, object>();
			parameters["text"] = "TALE";
			model.StartTrial(parameters);
			model.Update(0.5f);
			Assert.AreEqual("", model.helpTextNow.next);
			model.Update(0.5f);
			Assert.AreEqual(null, model.helpTextNow.next);
			model.Update(16.0f);
			string message = model.wordPositionMinNormal.ToString();
			Assert.AreEqual(model.shorterWordHelp, model.helpTextNow.next,
				message);
		}
	}
}
