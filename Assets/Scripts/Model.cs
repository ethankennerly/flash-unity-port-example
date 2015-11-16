using UnityEngine;
using System.Collections;

public class Model {
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
                        		// TODO prepareKnockback(submission.length, complete);
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

	public void Update(float deltaSeconds) {
	}
}
