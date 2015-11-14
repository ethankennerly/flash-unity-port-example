using System.Collections;

public class Levels {
	public int index = 0;

        /**
         * Some anagrams copied from:
         * http://www.enchantedlearning.com/english/anagram/numberofletters/5letters.shtml
         * Test case:  2015-04-18 Redbeard at The MADE types word.  Got stumped by anagram "ERISIOUS" and "NIOMTENTPO"
         * http://www.cse.unr.edu/~cohen/text.php
         */
	public ArrayList parameters = new ArrayList{
            new Hashtable(){{"text", "START"}, {"help", "ANAGRAM ATTACK\n\nCLICK HERE. TYPE \"START\".  PRESS THE SPACE KEY OR ENTER KEY."},
             {"wordWidthPerSecond", 0.0f},
             {"wordPosition", 0.0f}}
	};

	public Hashtable getParams() {
		return (Hashtable) parameters[index];
	}
}
