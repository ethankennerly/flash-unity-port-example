using UnityEngine;
using System.Collections;

public class Model {
	public string help;
        public int letterMax = 10;
	public Levels levels = new Levels();
	public string text;
	public ArrayList word;
	public ArrayList outputs = new ArrayList();
	public float wordPosition;
	public float wordWidthPerSecond;

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
		word = new ArrayList();
		char [] letters = text.ToCharArray();
		for (int i = 0; i < letters.Length; i++) {
			word.Add(letters[i].ToString());
		}
		Debug.Log("Model.trial: <" + word[0] + ">");
	}

	public void Update(float deltaSeconds) {
	}
}
