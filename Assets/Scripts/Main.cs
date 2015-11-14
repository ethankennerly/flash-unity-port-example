using UnityEngine;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	private Model model;

	public void Start() {
		model = new Model();
		model.Start();
	}

	public void Update() {
		model.Update(Time.deltaTime);
	}
}
