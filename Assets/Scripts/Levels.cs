using System.Collections;
using System.Collections.Generic;

using /*<com>*/Finegamedesign.Utils/*<DataUtil>*/;
namespace /*<com>*/Finegamedesign.Anagram
{
	public class Levels
	{
		internal int index = 0;

		/**
		 * Words by estimated difficulty will be appended.
		 * Test case:  2015-04-18 Redbeard at The MADE types word.  Got stumped by anagram "ERISIOUS" and "NIOMTENTPO"
		 * http://www.cse.unr.edu/~cohen/text.php
		 */
		internal List<Dictionary<string, dynamic>> parameters = new List<Dictionary<string, dynamic>>(){
			new Dictionary<string, dynamic>(){
				{
					"text", "START"}
				, {
					"help", "TAP THE LETTERS TO SPELL 'START.' THEN TAP 'SUBMIT.'"}
				,
				{
					"wordWidthPerSecond", 0.0f}
				,
				{
					"wordPosition", -200.0f}
			}
			,
			new Dictionary<string, dynamic>(){
				{
					"text", "SPELL"}
				, {
					"help", "USE ALL THE LETTERS.  FOR EXAMPLE:  'SPELL.'  THEN TAP 'SUBMIT.'"}
				,
				{
					"wordPosition", -100.0f}
			}
			,
			new Dictionary<string, dynamic>(){
				{
					"text", "WORDS"}
				, {
					"help", "SHORTER WORDS KNOCK THE LETTERS BACK. EXAMPLES: 'ROD', 'WORD'."}
				,
				{
					"wordPosition", -100.0f}
			}
			,
			new Dictionary<string, dynamic>(){
				{
					"text", "STARE"}
				, {
					"help", "YOU CAN USE EACH SHORT WORD ONCE. EXAMPLES: 'EAT', 'TEAR'."}
				,
				{
					"wordPosition", -100.0f}
			}
		}
		;
		
		internal Dictionary<string, dynamic> getParams()
		{
			return (Dictionary<string, dynamic>)parameters[index];
		}
		
		internal Dictionary<string, dynamic> progress(float fraction)
		{
			int add = (int)(fraction * DataUtil.Length(parameters));
			return up(add);
		}

		internal Dictionary<string, dynamic> up(int add = 1)
		{
			index = (index + add) % DataUtil.Length(parameters);
			while (index < 0)
			{
				index += DataUtil.Length(parameters);
			}
			return getParams();
		}
		
		internal int current()
		{
			return index + 1;
		}
		
		internal int count()
		{
			return DataUtil.Length(parameters);
		}
	}
}
