using System.Collections;

public class Words 
{
	public Hashtable words;

	public Hashtable init()
	{
		string text = System.IO.File.ReadAllText("Assets/Text/word_list_moby_crossword.flat.txt");
		string[] lines = text.Replace("\r\n", "\n").Split('\n');
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
