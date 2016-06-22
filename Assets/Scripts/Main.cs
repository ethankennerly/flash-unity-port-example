using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using /*<com>*/Finegamedesign.Anagram;

public class Main : MonoBehaviour {
	public AudioClip explosionBigSound;
	public AudioClip selectSound;
	public AudioClip shootSound;

	public float wordPosition;
	public float wordPositionScaled;

	private Model model;
	private AnagramView view;
	private Storage storage = new Storage();

	public void Start() {
		model = new Model();
		pushWords();
		model.wordHash = new Words().init();
		model.scaleToScreen(9.5f);
		model.load(storage.Load());
		view = new AnagramView(model, this);
		model.setReadingOrder(view.letterNodes);
	}

	private void pushWords()
	{
		string text = Toolkit.Read("text/anagram_words.txt");
		string[] words = Toolkit.Split(text, Toolkit.lineDelimiter);
		pushWords(model.levels.parameters, words);
		string[] win = new string[]{"YOU", "WIN"};
		pushWords(model.levels.parameters, win);
	}

	private static void pushWords(
			List<Dictionary<string, dynamic>> parameters, 
			string[] words)
	{
		for (int w = 0; w < words.Length; w++) {
			Dictionary<string, dynamic> 
			parameter = new Dictionary<string, dynamic>(){
				{"text", words[w]}};
			parameters.Add(parameter);
		}
	}

	public void Update() {
		model.update(Time.deltaTime);
		if ("complete" == model.state) {
			storage.SetKeyValue("level", model.progress.GetLevelNormal());
			storage.Save(storage.data);
		}
		wordPosition = model.wordPosition;
		wordPositionScaled = model.wordPositionScaled;
		view.update();
	}
}
