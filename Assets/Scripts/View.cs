using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class View {
	public int[] selects;
	private Model model;
	private GameObject main;

	public View(Model theModel, GameObject theMainScene) {
		model = theModel;
		main = theMainScene;
	}

	public bool IsLetterKeyDown(string letter)
	{
		return Input.GetKeyDown(letter.ToLower());
	}

	public void Update() {
		updateBackspace();
		ArrayList presses = model.getPresses(IsLetterKeyDown);
		updateSelect(model.press(presses), true);
		updateSubmit();
		updateLetters(main.transform.Find("word").gameObject.transform.Find("state").gameObject, model.word);
		updateLetters(main.transform.Find("input").gameObject.transform.Find("state").gameObject, model.inputs);
	}

        /**
         * Delete or backspace:  Remove last letter.
         */
        private void updateBackspace()
        {
            if (Input.GetKeyDown("delete")
            || Input.GetKeyDown("backspace"))
            {
                updateSelect(model.backspace(), false);
            }
        }

        /**
         * Each selected letter in word plays animation "selected".
Select, submit: Anders sees reticle and sword. Test case:  2015-04-18 Anders sees word is a weapon.
         */
        private void updateSelect(ArrayList selects, bool selected)
        {
	    if (1 <= selects.Count)
	    {
		    this.selects = (int[]) selects.ToArray(typeof(int));
	    }
            GameObject parent = main.transform.Find("word").gameObject.transform.Find("state").gameObject;
	string state = selected ? "selected" : "none";
            for (int s = 0; s < selects.Count; s++)
            {
                int index = (int) selects[s];
                string name = "Letter_" + index;
		Toolkit.setState(parent.transform.Find(name).gameObject, state);
                // TODO selectSound.play();
            }
        }


        /**
         * Press space or enter.  Input word.
         * Word robot approaches.
         *     restructure synchronized animations:
         *         word
         *             complete
         *             state
         *         input
         *             output
         *             state
         */
        private void updateSubmit()
        {
            if (Input.GetKeyDown("space")
            || Input.GetKeyDown("return"))
            {
                string state = model.submit();
                if (null != state) 
                {
                    // TODO main.word.gotoAndPlay(state);
                    // TODO main.input.gotoAndPlay(state);
                    // TODO shootSound.play();
                }
                resetSelect();
            }
        }

        private void resetSelect()
        {
            GameObject parent = main.transform.Find("word").gameObject.transform.Find("state").gameObject;
            int max = model.letterMax;
            for (int index = 0; index < max; index++)
            {
                string name = "Letter_" + index;
		Toolkit.setState(parent.transform.Find(name).gameObject, "none");
            }
        }

	/**
	 * Find could be cached.
	 * http://gamedev.stackexchange.com/questions/15601/find-all-game-objects-with-an-input-string-name-not-tag/15617#15617
	 */
        private void updateLetters(GameObject parent, ArrayList letters) {
		int max = model.letterMax;
		for (int i = 0; i < max; i++)
		{
			string name = "Letter_" + i;
			GameObject letter = parent.transform.Find(name).gameObject;
			if (null != letter)
			{
				bool visible = i < letters.Count;
				letter.SetActive(visible);
				if (visible) {
					GameObject text3d = letter.transform.Find("text3d").gameObject;
					TextMesh mesh = text3d.GetComponent<TextMesh>();
					mesh.text = (string) letters[i];
				}

			}
		}
	}
}
