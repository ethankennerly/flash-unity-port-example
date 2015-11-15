using System.Collections;

public class Toolkit
{
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
