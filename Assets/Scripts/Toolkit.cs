using UnityEngine;
using System.Collections;

public class Toolkit
{
	/**
	 * This was the most concise way I found to split a string without depending on other libraries.
	 * In ActionScript splitting a string is concise:  s.split("");
	 */
	public static ArrayList splitString(string text)
	{
		ArrayList available = new ArrayList();
		char [] letters = text.ToCharArray();
		for (int i = 0; i < letters.Length; i++) {
			available.Add(letters[i].ToString());
		}
		return available;
	}

	/**
	 * Call animator.Play instead of animator.SetTrigger, in case the animator is in transition.
	 * Test case:  2015-11-15 Enter "SAT".  Type "RAT".  Expect R selected.  Got R resets to unselected.
	 * http://answers.unity3d.com/questions/801875/mecanim-trigger-getting-stuck-in-true-state.html
	 */
	public static void setState(GameObject gameObject, string state)
	{
		Animator animator = gameObject.GetComponent<Animator>();
		if (null != animator && animator.isInitialized)
		{
			Debug.Log("Toolkit.setState: " + gameObject + ": " + state);
			animator.Play(state);
		}
	}
}
