using UnityEngine;
using System.Collections;

public class Toolkit
{
	private static string NormalizeLines(string text)
	{
		return text.Replace("\r\n", "\n");
	}

	/**
	 * @param	path	Unconventionally, Unity expects the file extension is omitted.  This utility will try again to remove file extension if it can't load the first time.
	 * Normalize line endings and trim whitespace.
	 * Expects path is relative to "Assets/Resources/" folder.
	 * Unity automatically embeds resource files.  Does not dynamically load file, because file system is incompatible on mobile device or HTML5.
	 */
	public static string Read(string path)
	{
		// string text = System.IO.File.ReadAllText(path);
		TextAsset asset = (TextAsset) Resources.Load(path);
		if (null == asset) {
			string basename = System.IO.Path.ChangeExtension(path, null);
			asset = (TextAsset) Resources.Load(basename);
			if (null == asset) {
				Debug.Log("Did you omit the file extension?  Did you place the file in the Assets/Resources/ folder?");
			}
		}
		string text = asset.text;
		text = NormalizeLines(text);
		text = text.Trim();
		// Debug.Log("Toolkit.Read: " + text);
		return text;
	}

	/**
	 * This was the most concise way I found to split a string without depending on other libraries.
	 * In ActionScript splitting a string is concise:  s.split("");
	 */
	public static ArrayList SplitString(string text)
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
	 * Test case:  2015-11-15 Enter "SAT".  Type "RAT".  Expect R selected.  Got "R" resets to unselected.
	 * http://answers.unity3d.com/questions/801875/mecanim-trigger-getting-stuck-in-true-state.html
	 *
	 * Do not call until initialized.  Test case:  2015-11-15 Got warning "Animator has not been initialized"
	 * http://answers.unity3d.com/questions/878896/animator-has-not-been-initialized-1.html
	 *
	 * In editor, deleted and recreated animator state transition.  Test case:  2015-11-15 Got error "Transition '' in state 'selcted' uses parameter 'none' which is not compatible with condition type"
	 * http://answers.unity3d.com/questions/1070010/transition-x-in-state-y-uses-parameter-z-which-is.html
	 */
	public static void setState(GameObject gameObject, string state, bool isRestart = true)
	{
		Animator animator = gameObject.GetComponent<Animator>();
		if (null != animator && animator.isInitialized)
		{
			// Debug.Log("Toolkit.setState: " + gameObject + ": " + state);
			if (isRestart)
			{
				animator.Play(state);
			}
			else
			{
				animator.Play(state, -1, 0f);
			}
		}
	}
}
