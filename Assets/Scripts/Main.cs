using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public AudioClip explosionBigSound;
	public AudioClip selectSound;
	public AudioClip shootSound;

	private Model model;
	private View view;

	public void Start() {
		model = new Model();
		view = new View(model, this);
	}

	public void Update() {
		model.update(Time.deltaTime);
		view.update();
	}
}
