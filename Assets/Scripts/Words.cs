using System.Collections.Generic;
using Finegamedesign.Utils;

namespace Finegamedesign.Anagram
{
	public sealed class Words 
	{
		public static Dictionary<string, object> words;

		public static Dictionary<string, object> Read()
		{
			string text = StringUtil.Read(
				"text/TWL06.txt"
			);
			string[] lines = text.Split('\n');
			int length = lines.Length;
			words = new Dictionary<string, object>();
			for (int i = 0; i < length; i++)
			{
				string word = lines[i];
				words[word] = true;
			}
			return words;
		}

		public static void Setup(AnagramModel model)
		{
			LoadWords(model);
			model.wordHash = Read();
		}

		private static void LoadWords(AnagramModel model)
		{
			string text = StringUtil.Read("text/anagram_words.txt");
			string[] words = StringUtil.Split(text, Toolkit.lineDelimiter);
			AddWords(model.levels.parameters, words);
			string[] win = new string[]{"YOU", "WIN"};
			AddWords(model.levels.parameters, win);
		}

		private static void AddWords(
				List<Dictionary<string, object>> parameters, 
				string[] words)
		{
			for (int w = 0; w < words.Length; w++) {
				Dictionary<string, object> 
				parameter = new Dictionary<string, object>(){
					{"text", words[w]}};
				parameters.Add(parameter);
			}
		}
	}
}
