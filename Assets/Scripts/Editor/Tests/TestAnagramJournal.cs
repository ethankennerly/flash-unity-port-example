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
			model.Submit();
			Assert.AreEqual("submit", model.journal.action);
		}
	}
}
