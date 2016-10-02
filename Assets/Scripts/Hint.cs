namespace Finegamedesign.Utils
{
	public sealed class Hint
	{
		public int count = 0;

		public string GetText()
		{
			return "HINT (" + count + ")";
		}
	}
}
