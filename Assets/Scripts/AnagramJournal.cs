using Finegamedesign.Utils;
using UnityEngine/*<Mathf>*/;

namespace Finegamedesign.Anagram
{
	public sealed class AnagramJournal
	{
		public string action = null;
		public int milliseconds = 0;
		public float seconds = 0.0f;

		public void Update(float deltaSeconds)
		{
			seconds += deltaSeconds;
			milliseconds = (int)(Mathf.Round(seconds * 1000.0f));
		}

		public void Record(string act)
		{
			action = act;
		}
	}
}
