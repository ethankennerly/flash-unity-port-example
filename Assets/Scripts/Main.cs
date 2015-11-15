using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public int[] selects;
	private Model model;
	private View view;

	public void Start() {
		model = new Model();
		view = new View(model, gameObject);
	}

	public void Update() {
		model.Update(Time.deltaTime);
		view.Update();
		selects = view.selects;
	}
}
