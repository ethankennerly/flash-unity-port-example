using System.Collections.Generic;

public class Words 
{
	public Dictionary<string, dynamic> words;

	public Dictionary<string, dynamic> init()
	{
		string text = Toolkit.Read("text/word_list_moby_crossword.flat.txt");
		string[] lines = text.Split('\n');
		int length = lines.Length;
		words = new Dictionary<string, dynamic>();
		for (int i = 0; i < length; i++)
		{
			string word = lines[i];
			words[word] = true;
		}
		return words;
	}
}
