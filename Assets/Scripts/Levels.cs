using System.Collections.Generic;

using /*<com>*/Finegamedesign.Utils/*<DataUtil>*/;
namespace /*<com>*/Finegamedesign.Anagram
{
	public class Levels
	{
		// Words by estimated difficulty will be appended.
		// Test case:  2015-04-18 Redbeard at The MADE types word.  Got stumped by anagram "ERISIOUS" and "NIOMTENTPO"
		// http://www.cse.unr.edu/~cohen/text.php
		internal List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>(){
			new Dictionary<string, object>(){
				{
					"text", "START"}
				, {
					"help", "TAP THE LETTERS TO SPELL 'START.' THEN TAP 'SUBMIT.'"}
				,
				{
					"wordWidthPerSecond", 0.0f}
			}
			,
			new Dictionary<string, object>(){
				{
					"text", "SPELL"}
				, {
					"help", "USE ALL THE LETTERS.  FOR EXAMPLE:  'SPELL.'  THEN TAP 'SUBMIT.'"}
			}
			,
			new Dictionary<string, object>(){
				{
					"text", "WORDS"}
				, {
					"help", "TO ADD TIME, SPELL A SHORT WORD. EXAMPLES: 'ROD', 'WORD'."}
			}
		}
		;
	}
}
