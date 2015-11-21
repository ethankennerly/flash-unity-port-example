using UnityEngine;  // Mathf
using System.Collections;
using System;  // Random;

public class Model {

	private static System.Random rng = new System.Random();

	/**
	 * Unity Random() includes 0.0 and 1.0 which would be out of index range.
	 * http://docs.unity3d.com/ScriptReference/Random-value.html
	 */
        private static void shuffle(ArrayList cards)
        {
            for (int i = cards.Count - 1; 1 <= i; i--)
            {
                int r = (int) (rng.NextDouble() * (i + 1f));
                object swap = cards[r];
                cards[r] = cards[i];
                cards[i] = swap;
            }
        }

	public ArrayList completes = new ArrayList();
	public string help;
	public string helpState;
	public int letterMax = 10;
	public ArrayList inputs = new ArrayList();
	public delegate void ActionDelegate();
	public ActionDelegate onComplete;
	public Levels levels = new Levels();
	public ArrayList outputs = new ArrayList();
	public int points = 0;
	public int score = 0;
	public string state;
	public string text;
	public ArrayList word;
	public float wordPosition;
	public float wordWidthPerSecond;

	private ArrayList available;
	private ArrayList selects;
	private Hashtable repeat = new Hashtable();
	private Hashtable wordHash;

	public delegate bool IsJustPressed(string letter);

	public Model()
	{
		wordHash = new Words().init();
		trial(levels.getParams());
	}

	private void trial(Hashtable parameters) {
		wordPosition = 0.0f;
		help = "";
		wordWidthPerSecond = -0.01f;
		if (null != parameters["text"]) {
			text = (string) parameters["text"];
		}
		if (null != parameters["help"]) {
			help = (string) parameters["help"];
		}
		if (null != parameters["wordWidthPerSecond"]) {
			wordWidthPerSecond = (float) parameters["wordWidthPerSecond"];
		}
		if (null != parameters["wordPosition"]) {
			wordPosition = (float) parameters["wordPosition"];
		}
		available = Toolkit.splitString(text);
		word = (ArrayList) available.Clone();
		selects = (ArrayList) word.Clone();
		repeat = new Hashtable();
		// Debug.Log("Model.trial: word[0]: <" + word[0] + ">");
	}

	/**
	 * @param   justPressed	 Filter signature justPressed(letter):Boolean.
	 */
	public ArrayList getPresses(IsJustPressed justPressed) 
	{
		ArrayList presses = new ArrayList();
		Hashtable letters = new Hashtable();
		for (int i = 0; i < available.Count; i++) {
			string letter = (string) available[i];
			if (letters.ContainsKey(letter)) 
			{
				continue;
			}
			else
			{
				letters[letter] = true;
			}
			if (justPressed(letter))
			{
				presses.Add(letter);
			}
		}
		return presses;
	}

	/**
	 * If letter not available, disable typing it.
	 * @return array of word indexes.
	 */
	public ArrayList press(ArrayList presses)
	{
		Hashtable letters = new Hashtable();
		ArrayList selectsNow = new ArrayList();
		for (int i = 0; i < presses.Count; i++)
		{
			string letter = (string) presses[i];
			if (letters.ContainsKey(letter))
			{
				continue;
			}
			else
			{
				letters[letter] = true;
			}
			int index = available.IndexOf(letter);
			if (0 <= index)
			{
				available.RemoveRange(index, 1);
				inputs.Add(letter);
				int selected = selects.IndexOf(letter);
				if (0 <= selected)
				{
					selectsNow.Add(selected);
					selects[selected] = letter.ToLower();
				}
			}
		}
		return selectsNow;
	}

        public ArrayList backspace()
        {
            ArrayList selectsNow = new ArrayList();
            if (1 <= inputs.Count)
            {
                string letter = (string) inputs[inputs.Count - 1];
                inputs.RemoveAt(inputs.Count - 1);
                available.Add(letter);
                int selected = selects.LastIndexOf(letter.ToLower());
                if (0 <= selected)
                {
                    selectsNow.Add(selected);
                    selects[selected] = letter;
                }
            }
            return selectsNow;
        }

	/**
	 * @return animation state.
	 *	  "submit" or "complete":  Word shoots. Test case:  2015-04-18 Anders sees word is a weapon.
	 *	  "submit":  Shuffle letters.  Test case:  2015-04-18 Jennifer wants to shuffle.  Irregular arrangement of letters.  Jennifer feels uncomfortable.
	 * Test case:  2015-04-19 Backspace. Deselect. Submit. Type. Select.
	 */
	public string submit()
	{
		string submission = string.Join("", ((string []) inputs.ToArray(typeof(string))));
		bool accepted = false;
		if (1 <= submission.Length)
		{
			if (wordHash.ContainsKey(submission))
			{
				if (repeat.ContainsKey(submission))
				{
					state = "repeat";
					if (levels.index <= 50 && "" == help)
					{
						help = "YOU CAN ONLY ENTER EACH SHORTER WORD ONCE.";
						helpState = "repeat";
					}
				}
				else
				{
					if ("repeat" == helpState)
					{
						helpState = "";
						help = "";
					}
					repeat[submission] = true;
					accepted = true;
                        		scoreUp(submission);
					bool complete = text.Length == submission.Length;
                        		prepareKnockback(submission.Length, complete);
					if (complete)
					{
						completes = (ArrayList) word.Clone();
						trial(levels.up());
						state = "complete";
						if (null != onComplete)
						{
							onComplete();
						}
					}
					else
					{
						state = "submit";
					}
				}
			}
			outputs = (ArrayList) inputs.Clone();
		}
		// Debug.Log("Model.submit: " + submission + ". Accepted " + accepted);
		inputs.RemoveRange(0, inputs.Count);
		available = (ArrayList) word.Clone();
		selects = (ArrayList) word.Clone();
		return state;
	}

        private void scoreUp(string submission)
        {
            points = submission.Length;
            score += points;
	    // Debug.Log("Model.scoreUp: points " + points + " increase score to " + score);
        }

	public void update(float deltaSeconds) 
	{
		updatePosition(deltaSeconds);	
	}

        public float width = 720f;

        private void clampWordPosition()
        {
            float wordWidth = 160f;
            float min = wordWidth - width;
            if (wordPosition <= min)
            {
                help = "GAME OVER!"; //  TO SKIP ANY WORD, PRESS THE PAGEUP KEY.  TO GO BACK A WORD, PRESS THE PAGEDOWN KEY.";
                helpState = "gameOver";
            }
            wordPosition = Mathf.Max(min, Mathf.Min(0f, wordPosition));
        }

        private void updatePosition(float seconds)
        {
            wordPosition += (seconds * width * wordWidthPerSecond);
            clampWordPosition();
            //- Debug.Log("Model.updatePosition: " + wordPosition);
        }

        private float outputKnockback = 0.0f;

        public bool mayKnockback()
        {
            return 0 < outputKnockback && 1 <= outputs.Count;
        }

        /**
         * Clamp word to appear on screen.  Test case:  2015-04-18 Complete word.  See next word slide in.
         */
        private void prepareKnockback(int length, bool complete)
        {
            float perLength = 
                                   0.03f;
                                   // 0.05f;
                                   // 0.1f;
            outputKnockback = perLength * width * length;
            if (complete) {
                outputKnockback *= 3f;
            }
            clampWordPosition();
        }

        public bool onOutputHitsWord()
        {
            bool enabled = mayKnockback();
            if (enabled)
            {
                wordPosition += outputKnockback;
                shuffle(word);
                selects = (ArrayList) word.Clone();
                for (int i = 0; i < inputs.Count; i++)
                {
                    string letter = (string) inputs[i];
                    int selected = selects.IndexOf(letter);
                    if (0 <= selected)
                    {
                        selects[selected] = letter.ToLower();
                    }
                }
                outputKnockback = 0;
            }
            return enabled;
        }
}
