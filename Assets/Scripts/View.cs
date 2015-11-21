using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class View {
	private AudioSource audio;
	private Model model;
	private Main main;

	public View(Model theModel, Main theMainScene) {
		model = theModel;
		main = theMainScene;
		audio = main.gameObject.GetComponent<AudioSource>();
	}

	public bool IsLetterKeyDown(string letter)
	{
		return Input.GetKeyDown(letter.ToLower());
	}

	public void update() {
		updateBackspace();
		ArrayList presses = model.getPresses(IsLetterKeyDown);
		updateSelect(model.press(presses), true);
		updateSubmit();
		updatePosition();
		updateLetters(main.transform.Find("word/state").gameObject, model.word);
		updateLetters(main.transform.Find("input/state").gameObject, model.inputs);
		updateLetters(main.transform.Find("input/output").gameObject, model.outputs);
		updateHud();
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
            GameObject parent = main.transform.Find("word/state").gameObject;
	string state = selected ? "selected" : "none";
            for (int s = 0; s < selects.Count; s++)
            {
                int index = (int) selects[s];
                string name = "Letter_" + index;
		Toolkit.setState(parent.transform.Find(name).gameObject, state);
                audio.PlayOneShot(main.selectSound);
            }
        }

	private void updateHud()
	{
		Text help = main.transform.Find("hud/help").GetComponent<Text>();
		help.text = model.help.ToString();
		Text score = main.transform.Find("hud/score").GetComponent<Text>();
		score.text = model.score.ToString();
		Text level = main.transform.Find("hud/level").GetComponent<Text>();
		level.text = model.levels.current().ToString();
		Text levelMax = main.transform.Find("hud/levelMax").GetComponent<Text>();
		levelMax.text = model.levels.count().ToString();
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
		    Toolkit.setState(main.transform.Find("input").gameObject, "submit");
		    audio.PlayOneShot(main.shootSound);
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
	 * Unity prohibits editing a property of position.
	 */
	private void setPositionY(Transform transform, float y)
	{
		Vector3 position = transform.position;
		position.y = y;
		transform.position = position;
	}

	/**
	 * Unity prohibits editing a property of position.
	 */
	private void updatePosition()
	{
		if (model.mayKnockback())
		{
			updateOutputHitsWord();
		}
		setPositionY(main.transform.Find("word"), model.wordPosition * 0.0325f);
	}

	private void updateOutputHitsWord()
	{
                if (model.onOutputHitsWord())
		{
			audio.PlayOneShot(main.explosionBigSound);
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
