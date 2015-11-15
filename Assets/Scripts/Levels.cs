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
             {"wordPosition", 0.0f}},
            new Hashtable(){{"text", "LSEPL"}, {"help", "TO ADVANCE, USE ALL THE LETTERS.  HINT:  \"SPELL\".  THEN PRESS THE SPACE KEY OR ENTER KEY."}},
            new Hashtable(){{"text", "DWORS"}, {"help", "SHORTER WORDS SHUFFLE SAME LETTERS. EXAMPLES: \"ROD\", \"RODS\", \"WORD\", \"SWORD\"."}},
            new Hashtable(){{"text", "STARE"}, {"help", "SHORTER WORDS KNOCKBACK.  YOU CAN USE EACH SHORT WORD ONCE. EXAMPLE:  \"EAT\", \"TEAR\", \"STARE\""}},
            new Hashtable(){{"text", "FOR"}, {"help", "WORDS WITH FEW LETTERS MOVE FAST!"}},
            new Hashtable(){{"text", "EAT"}},
            new Hashtable(){{"text", "ART"}},
            new Hashtable(){{"text", "SAP"}},
            new Hashtable(){{"text", "SATE"}},
            new Hashtable(){{"text", "APT"}},
            new Hashtable(){{"text", "ARM"}},
            new Hashtable(){{"text", "ERA"}},
            new Hashtable(){{"text", "POST"}},
            new Hashtable(){{"text", "OWN"}},
            new Hashtable(){{"text", "PLEA"}},
            new Hashtable(){{"text", "BATS"}},
            new Hashtable(){{"text", "LEAD"}},
            new Hashtable(){{"text", "BEAST"}, {"help", "FOR BONUS POINTS, FIRST ENTER SHORT WORDS.  EXAMPLES: \"BE\", \"BATS\", \"AT\"."}},
            new Hashtable(){{"text", "DIET"}},
            new Hashtable(){{"text", "INKS"}},
            new Hashtable(){{"text", "LIVE"}},
            new Hashtable(){{"text", "RACES"}, {"help", "TOO CLOSE?  ENTER SHORT WORDS."}},
            new Hashtable(){{"text", "KALE"}},
            new Hashtable(){{"text", "SNOW"}},
            new Hashtable(){{"text", "NEST"}},
            new Hashtable(){{"text", "STEAM"}},
            new Hashtable(){{"text", "EMIT"}},
            new Hashtable(){{"text", "NAME"}},
            new Hashtable(){{"text", "SWAY"}},
            new Hashtable(){{"text", "PEARS"}},
            new Hashtable(){{"text", "SKATE"}},
            new Hashtable(){{"text", "BREAD"}},
            new Hashtable(){{"text", "CODE"}},
            new Hashtable(){{"text", "DIETS"}},
            new Hashtable(){{"text", "CRATES"}, {"help", "SHORT WORDS SHUFFLES LETTERS, BUT THEY REMAIN THE SAME."}},
            new Hashtable(){{"text", "TERSE"}},
            new Hashtable(){{"text", "LAPSE"}},
            new Hashtable(){{"text", "PROSE"}},
            new Hashtable(){{"text", "SPREAD"}, {"help", "FOR BONUS POINTS OR KNOCKBACK, ENTER SHORT WORDS. TO ADVANCE, ENTER FULL WORD."}},
            new Hashtable(){{"text", "SMILE"}},
            new Hashtable(){{"text", "ALERT"}},
            new Hashtable(){{"text", "BEGIN"}},
            new Hashtable(){{"text", "TIMERS"}},
            new Hashtable(){{"text", "HEROS"}},
            new Hashtable(){{"text", "PETAL"}},
            new Hashtable(){{"text", "LITER"}},
            new Hashtable(){{"text", "PETALS"}},
            new Hashtable(){{"text", "VERSE"}},
            new Hashtable(){{"text", "RESIN"}},
            new Hashtable(){{"text", "NOTES"}},
            new Hashtable(){{"text", "SHEAR"}},
            new Hashtable(){{"text", "SUBTLE"}},
            new Hashtable(){{"text", "SPARSE"}},
            new Hashtable(){{"text", "REWARD"}},
            new Hashtable(){{"text", "REPLAYS"}, {"help", "NEXT SESSION, TO SKIP WORDS, PRESS PAGEUP."}},
            new Hashtable(){{"text", "MANTEL"}},
            new Hashtable(){{"text", "DESIGN"}},
            new Hashtable(){{"text", "LASTED"}},
            new Hashtable(){{"text", "RECANTS"}},
            new Hashtable(){{"text", "FOREST"}},
            new Hashtable(){{"text", "POINTS"}},
            new Hashtable(){{"text", "MASTER"}},
            new Hashtable(){{"text", "THREADS"}},
            new Hashtable(){{"text", "DANGER"}},
            new Hashtable(){{"text", "SPRITES"}},
            new Hashtable(){{"text", "ARTIST"}},
            new Hashtable(){{"text", "TENSOR"}},
            new Hashtable(){{"text", "ARIDEST"}},
            new Hashtable(){{"text", "LISTEN"}},
            new Hashtable(){{"text", "PIRATES"}},
            new Hashtable(){{"text", "ALERTED"}},
            new Hashtable(){{"text", "ALLERGY"}},
            new Hashtable(){{"text", "REDUCES"}},
            new Hashtable(){{"text", "MEDICAL"}},
            new Hashtable(){{"text", "RAPIDS"}},
            new Hashtable(){{"text", "RETARDS"}},
            new Hashtable(){{"text", "REALIST"}},
            new Hashtable(){{"text", "MEANEST"}},
            new Hashtable(){{"text", "ADMIRER"}},
            new Hashtable(){{"text", "TRAINERS"}},
            new Hashtable(){{"text", "RECOUNTS"}},
            new Hashtable(){{"text", "PARROTED"}},
            new Hashtable(){{"text", "DESIGNER"}},
            new Hashtable(){{"text", "CRATERED"}},
            new Hashtable(){{"text", "CALIPERS"}},
            new Hashtable(){{"text", "CREATIVE"}},
            new Hashtable(){{"text", "ARROGANT"}},
            new Hashtable(){{"text", "EMIGRANTS"}},
            new Hashtable(){{"text", "AUCTIONED"}},
            new Hashtable(){{"text", "CASSEROLE"}},
            new Hashtable(){{"text", "UPROARS"}},
            new Hashtable(){{"text", "ANTIGEN"}},
            new Hashtable(){{"text", "DEDUCTIONS"}},
            new Hashtable(){{"text", "INTRODUCES"}},
            new Hashtable(){{"text", "PERCUSSION"}},
            new Hashtable(){{"text", "CONFIDENT"}},
            new Hashtable(){{"text", "HARMONICAS"}},
            new Hashtable(){{"text", "OMNIPOTENT"}},
            new Hashtable(){{"text", "YOU"}},
            new Hashtable(){{"text", "WIN"}}
	};

	public Hashtable getParams() {
		return (Hashtable) parameters[index];
	}

	public Hashtable up(int add = 1)
	{
		index = (index + add) % parameters.Count;
		while (index < 0)
		{
			index += parameters.Count;
		}
		return getParams();
	}
}
