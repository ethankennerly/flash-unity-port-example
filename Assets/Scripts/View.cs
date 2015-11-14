using UnityEngine;
using System.Collections.Generic;

public class View {
	private Model model;
	private GameObject main;

	public void Start(Model theModel, GameObject theMainScene) {
		model = theModel;
		main = theMainScene;
	}

	public void Update() {
	}

        private void updateLetters(GameObject parent, List<string> letters) {
	}
}
