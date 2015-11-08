using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class AnagramView {
	private AnagramModel model;
	private GameObject main;

	public void Start(AnagramModel theModel, GameObject theMainScene) {
		model = theModel;
		main = theMainScene;
	}

	public void Update() {
	}

        private void updateLetters(GameObject parent, List<string> letters) {
	}
}
