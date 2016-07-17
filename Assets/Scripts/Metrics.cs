using UnityEngine;
using System.Collections.Generic;

namespace /*<com>*/Finegamedesign.Utils
{
	public class Metrics
	{
		// Divided by integer and string data type.
		// Integers are simpler than floats.
		// Sometimes a string is useful, though slower to query.
		// Not object type, because MonoDevelop-Unity 5.3 debugger 
		// crashes when it encounters a object keyword.
		public Dictionary<string, int> trial_integers;
		public Dictionary<string, string> trial_strings;
		public List<Dictionary<string, int>> trials_integers;
		public List<Dictionary<string, string>> trials_strings;
		private int trial_begin = -1;
		private int time = 0;
		public string trial_tsv;
		public string column_separator = "\t";
		public string row_separator = "\n";
		public string[] trial_headers = new string[]{};

		public void StartSession()
		{
			if (null == trials_integers)
			{
				trials_integers = new List<Dictionary<string, int>>();
			}
			if (null == trials_strings)
			{
				trials_strings = new List<Dictionary<string, string>>();
			}
		}

		public void EndSession()
		{
			trial_tsv = ToTable();
			Toolkit.Log(
				"Metrics.EndSession: Format to copy and paste into a spreadsheet:"
				+ "\n" + trial_tsv);
		}

		public void StartTrial()
		{
			trial_begin = time;
			trial_integers = new Dictionary<string, int>();
			trials_integers.Add(trial_integers);
			trial_strings = new Dictionary<string, string>();
			trials_strings.Add(trial_strings);
			trial_integers["response_time"] = -1;
		}

		public void EndTrial()
		{
			trial_integers["response_time"] = time - trial_begin;
		}

		public void Update(float deltaSeconds)
		{
			int step = (int)(Mathf.Round(deltaSeconds * 1000.0f));
			time += step;
		}

		public string ToTable()
		{
			string table = "";
			string row = "";
			int index;
			string header;
			for (index = 0; index < trial_headers.Length; index++) {
				if ( 1 <= index) {
					row += column_separator;
				}
				header = trial_headers[index];
				row += header;
			}
			table += row;
			table += row_separator;
			for (int i = 0; i < trials_integers.Count; i++) {
				row = "";
				for (index = 0; index < trial_headers.Length; index++) {
					if ( 1 <= index) {
						row += column_separator;
					}
					string column;
					header = trial_headers[index];
					if (trials_integers[i].ContainsKey(header)) {
						column = trials_integers[i][header].ToString();
					}
					else if (trials_strings[i].ContainsKey(header)) {
						column = trials_strings[i][header];
					}
					else {
						// throw new System.Exception
						Toolkit.Log("Expected " + header + " in " + trials_integers[i] + " or " + trials_strings[i]);
						column = "-1";
					}
					row += column;
				}
				table += row;
				table += row_separator;
			}
			return table;
		}
	}
}
