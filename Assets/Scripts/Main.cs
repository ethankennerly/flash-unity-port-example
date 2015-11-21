using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public AudioClip explosionBigSound;
	public AudioClip selectSound;
	public AudioClip shootSound;

	public float wordPosition;
	public float wordPositionScaled;

	private Model model;
	private View view;

	public void Start() {
		model = new Model();
		model.scaleToScreen(9.5f);
		view = new View(model, this);
	}

	public void Update() {
		model.update(Time.deltaTime);
		wordPosition = model.wordPosition;
		wordPositionScaled = model.wordPositionScaled;
		view.update();
	}
}
