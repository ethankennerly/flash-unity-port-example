using System.Collections;
using System.Collections.Generic;

using /*<com>*/Finegamedesign.Utils/*<DataUtil>*/;
namespace /*<com>*/Finegamedesign.Anagram.TestSyntax
{
    public class TestSyntaxLevels
    {
        
        internal int index = 0;
        /**
         * Some anagrams copied from:
         * http://www.enchantedlearning.com/english/anagram/numberofletters/5letters.shtml
         * Test case:  2015-04-18 Redbeard at The MADE types word.  Got stumped by anagram "ERISIOUS" and "NIOMTENTPO"
         * http://www.cse.unr.edu/~cohen/text.php
         */
        internal ArrayList parameters = new ArrayList(){
            new Dictionary<string, object>(){
                {
                    "text", "START"}
                , {
                    "help", "ANAGRAM ATTACK\n\nCLICK HERE. TYPE 'START'.  PRESS THE SPACE KEY OR ENTER KEY."}
                ,
                {
                    "wordWidthPerSecond", 0.0f}
                ,
                {
                    "wordPosition", 0.0f}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LSEPL"}
                , {
                    "help", "TO ADVANCE, USE ALL THE LETTERS.  HINT:  'SPELL'.  THEN PRESS THE SPACE KEY OR ENTER KEY."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DWORS"}
                , {
                    "help", "SHORTER WORDS SHUFFLE SAME LETTERS. EXAMPLES: 'ROD', 'RODS', 'WORD', 'SWORD'."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "STARE"}
                , {
                    "help", "SHORTER WORDS KNOCKBACK.  YOU CAN USE EACH SHORT WORD ONCE. EXAMPLE:  'EAT', 'TEAR', 'STARE'"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "FOR"}
                , {
                    "help", "WORDS WITH FEW LETTERS MOVE FAST!"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "EAT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ART"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SAP"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SATE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "APT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ARM"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ERA"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "POST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "OWN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PLEA"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "BATS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LEAD"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "BEAST"}
                , {
                    "help", "FOR BONUS POINTS, FIRST ENTER SHORT WORDS.  EXAMPLES: 'BE', 'BATS', 'AT'."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DIET"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "INKS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LIVE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RACES"}
                , {
                    "help", "TOO CLOSE?  ENTER SHORT WORDS."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "KALE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SNOW"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "NEST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "STEAM"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "EMIT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "NAME"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SWAY"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PEARS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SKATE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "BREAD"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CODE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DIETS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CRATES"}
                , {
                    "help", "SHORT WORDS SHUFFLES LETTERS, BUT THEY REMAIN THE SAME."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "TERSE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LAPSE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PROSE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SPREAD"}
                , {
                    "help", "FOR BONUS POINTS OR KNOCKBACK, ENTER SHORT WORDS. TO ADVANCE, ENTER FULL WORD."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SMILE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ALERT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "BEGIN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "TIMERS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "HEROS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PETAL"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LITER"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PETALS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "VERSE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RESIN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "NOTES"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SHEAR"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SUBTLE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SPARSE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "REWARD"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "REPLAYS"}
                , {
                    "help", "NEXT SESSION, TO SKIP WORDS, PRESS PAGEUP."}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "MANTEL"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DESIGN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LASTED"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RECANTS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "FOREST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "POINTS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "MASTER"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "THREADS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DANGER"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "SPRITES"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ARTIST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "TENSOR"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ARIDEST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "LISTEN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PIRATES"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ALERTED"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ALLERGY"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "REDUCES"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "MEDICAL"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RAPIDS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RETARDS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "REALIST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "MEANEST"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ADMIRER"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "TRAINERS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "RECOUNTS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PARROTED"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DESIGNER"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CRATERED"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CALIPERS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CREATIVE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ARROGANT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "EMIGRANTS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "AUCTIONED"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CASSEROLE"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "UPROARS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "ANTIGEN"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "DEDUCTIONS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "INTRODUCES"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "PERCUSSION"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "CONFIDENT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "HARMONICAS"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "OMNIPOTENT"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "YOU"}
            }
            ,
            new Dictionary<string, object>(){
                {
                    "text", "WIN"}
            }
        }
        ;
        
        internal Dictionary<string, object> GetParams()
        {
            return (Dictionary<string, object>)(parameters[index]);
        }
        
        internal Dictionary<string, object> Up(int add = 1)
        {
            index = (index + add) % DataUtil.Length(parameters);
            while (index < 0)
            {
                index += DataUtil.Length(parameters);
            }
            return GetParams();
        }
        
        internal int Current()
        {
            return index + 1;
        }
        
        internal int Count()
        {
            return DataUtil.Length(parameters);
        }
    }
}
