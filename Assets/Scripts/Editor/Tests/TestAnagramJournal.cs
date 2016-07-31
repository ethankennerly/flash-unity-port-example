using NUnit.Framework;

namespace Finegamedesign.Anagram
{
	[TestFixture]
	public sealed class TestAnagramJournal
	{
		[Test]
		public void SubmitAction()
		{
			AnagramModel model = new AnagramModel();
			model.Update(1.0f / 60.0f);
			model.Update(1.0f / 60.0f);
			model.Submit();
			Assert.AreEqual("submit", model.journal.action);
			Assert.AreEqual(33, model.journal.milliseconds);
		}
	}
}
