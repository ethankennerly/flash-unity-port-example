using Finegamedesign.Utils;

namespace Finegamedesign.Anagram
{
	public sealed class AnagramJournal
	{
		public string action = null;

		public void Record(string act)
		{
			action = act;
		}
	}
}
