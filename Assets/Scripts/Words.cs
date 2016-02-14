using System.Collections;

public class Words 
{
	public Hashtable words;

	public Hashtable init()
	{
		string text = Toolkit.Read("text/word_list_moby_crossword.flat.txt");
		string[] lines = text.Split('\n');
		int length = lines.Length;
		words = new Hashtable();
		for (int i = 0; i < length; i++)
		{
			string word = lines[i];
			words[word] = true;
		}
		return words;
	}
}
