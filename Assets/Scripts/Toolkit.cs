using System.Collections;

public class Toolkit
{
	/**
	 * This was the most concise way I found to split a string without depending on other libraries.
	 * In ActionScript splitting a string is concise:  s.split("");
	 */
	public static ArrayList splitString(string text)
	{
		ArrayList available = new ArrayList();
		char [] letters = text.ToCharArray();
		for (int i = 0; i < letters.Length; i++) {
			available.Add(letters[i].ToString());
		}
		return available;
	}
}
