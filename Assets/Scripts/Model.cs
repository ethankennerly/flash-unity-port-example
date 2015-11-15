using UnityEngine;
using System.Collections;

public class Model {
	public string help;
	public int letterMax = 10;
	public ArrayList inputs = new ArrayList();
	public Levels levels = new Levels();
	public string text;
	public ArrayList word;
	public ArrayList outputs = new ArrayList();
	public float wordPosition;
	public float wordWidthPerSecond;

	private ArrayList available;
	private ArrayList selects;

	public delegate bool IsJustPressed(string letter);

	public Model()
	{
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
		Debug.Log("Model.trial: <" + word[0] + ">");
	}

	/**
	 * @param   justPressed     Filter signature justPressed(letter):Boolean.
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
		return presses;
	}

	public void Update(float deltaSeconds) {
	}
}
