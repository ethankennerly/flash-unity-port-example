using NUnit.Framework/*<Test>*/;

namespace Finegamedesign.Anagram
{
	public sealed class TestAnagramModel
	{
		[Test]
		public void IsTrialCycleNow()
		{
			AnagramModel model = new AnagramModel();
			model.progress.level = model.tutorLevel;
			Assert.AreEqual(false, model.IsTutor(), "tutor");
			model.trialPeriod = 10;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 11;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 19;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 20;
			Assert.AreEqual(true, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 21;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
		}

		[Test]
		public void IsTrialCycleNowTutorial()
		{
			AnagramModel model = new AnagramModel();
			Assert.AreEqual(true, model.IsTutor(), "tutor");
			model.trialPeriod = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 4;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 5;
			Assert.AreEqual(true, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), model.trialPeriod.ToString());
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), model.trialPeriod.ToString());
		}
	}
}
