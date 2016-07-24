using UnityEngine;
using System.Collections.Generic;

using /*<com>*/Finegamedesign.Utils/*<Model>*/;
namespace /*<com>*/Finegamedesign.Anagram
{
	public sealed class Model
	{
		private static bool ContainsWord(Dictionary<string, object> wordHash, List<string> letters, List<int> readingOrder)
		{
			string word = "";
			for (int index = 0; index < DataUtil.Length(readingOrder); index++)
			{
				if (index < DataUtil.Length(letters))
				{
					word += letters[index];
				}
			}
			return wordHash.ContainsKey(word);
		}

		private static void shuffleNotWord(Dictionary<string, object> wordHash, List<string> letters, List<int> readingOrder, int attemptMax = 16)
		{
			for (int attempt = 0; attempt < attemptMax; attempt++)
			{
				Deck.ShuffleList(letters);
				if (!ContainsWord(wordHash, letters, readingOrder)) {
					break;
				}
			}
		}
		
		internal List<int> stationIndexes;
		internal string helpState;
		internal int letterMax = 10;
		internal List<string> inputs = new List<string>();
		/**
		 * From letter graphic.
		 */
		internal float letterWidth = 42.0f;
		internal float wordWidth = 
									// 160.0f;
									420.0f; 
		internal delegate /*<object>*/void ActionDelegate();
		internal /*<Function>*/ActionDelegate onComplete;
		internal delegate bool IsJustPressed(string letter);
		internal string help = "";
		internal List<string> outputs = new List<string>();
		internal List<string> hints = new List<string>();
		private string hintWord;
		internal bool isHintVisible = false;
		internal int submitsUntilHint = 1; // 3;
		internal int submitsUntilHintNow;
		internal bool isContinueVisible = false;
		internal bool isGamePlaying = false;
		internal bool isNewGameVisible = false;
		internal List<string> completes = new List<string>();
		internal string text;
		internal List<string> word;
		internal float progressPositionScaled = 0.0f;
		internal float progressPositionTweened = 0.0f;
		internal float wordPosition = 0.0f;
		internal float wordPositionScaled = 0.0f;
		internal float checkpointStep = 0.1f;
		internal int points = 0;
		internal int score = 0;
		internal int tutorLevel = 0;
		internal bool isHudVisible = false;
		internal string state;
		internal string wordStateNow;
		internal Levels levels = new Levels();
		internal Progress progress = new Progress();
		private List<string> available;
		private Dictionary<string, object> repeat = new Dictionary<string, object>(){
		}
		;
		private List<string> selects;
		internal Dictionary<string, object> wordHash;
		private bool isVerbose = true;
		private float responseSeconds;
		private float wordPositionMin;
		private float checkpointInterval = -16.0f; 
		private float progressScale;
		private int trialCount;
		internal Metrics metrics = new Metrics();
		private int previous = 0;
		private int now = 0;
		internal float width = 720;
		internal float scale = 1.0f;
		private float wordWidthPerSecond;
		
		public Model()
		{
			setupProgress();
			tutorLevel = 3;
			trialCount = 0;
			isNewGameVisible = true;
			populateWord("");
			metrics.trial_headers = new string[]{
				"level_start", 
				"level_up", 
				"response_time", 
				"game_over", 
				"hint_count", 
				"word" 
			};
			metrics.StartSession();
		}
		
		private void populateWord(string text)
		{
			this.text = text;
			available = DataUtil.Split(text, "");
			word = DataUtil.CloneList(available);
			populateHint(text);
			stationIndexes = new List<int>();
			for (int index = 0; index < DataUtil.Length(word); index++)
			{
				stationIndexes.Add(index);
			}
		}

		internal void trial(Dictionary<string, object> parameters)
		{
			isGamePlaying = true;
			isContinueVisible = false;
			isNewGameVisible = false;
			responseSeconds = 0.0f;
			wordPosition = 0.0f;
			wordPositionMin = 0.0f;
			help = "";
			wordWidthPerSecond = isTutor() ? -0.005f : -0.01f;
			if (parameters.ContainsKey("text")) {
				text = (string)parameters["text"];
			}
			if (parameters.ContainsKey("help")) {
				help = (string)parameters["help"];
			}
			if (parameters.ContainsKey("wordWidthPerSecond")) {
				wordWidthPerSecond = (float)parameters["wordWidthPerSecond"];
			}
			if (parameters.ContainsKey("wordPosition")) {
				wordPosition = (float)parameters["wordPosition"];
			}
			populateWord(text);
			if ("" == help)
			{
				//? shuffleNotWord(wordHash, word, readingOrder);
				wordWidthPerSecond = // -0.05;
				// -0.02;
				// -0.01;
				// -0.005;
				// -0.002f;
				// -0.001;
				-0.002f * (width - wordWidth) / width;
				float power =
				// 1.5;
				// 1.75;
				2.0f;
				int baseRate = Mathf.Max(1, letterMax - DataUtil.Length(text));
				wordWidthPerSecond *= Mathf.Pow(baseRate, power);
			}
			selects = DataUtil.CloneList(word);
			repeat = new Dictionary<string, object>(){
			}
			;
			wordStateNow = "begin";
			if (isVerbose) 
			{
				Debug.Log("Model.trial: word[0]: <" + word[0] + ">" 
					+ " level " + progress.GetLevelNormal());
			}
			metrics.StartTrial();
			metrics.trial_integers["game_over"] = 0;
			metrics.trial_integers["hint_count"] = 0;
			metrics.trial_integers["level_start"] = progress.GetLevelNormal();
			metrics.trial_integers["level_up"] = 0;
			metrics.trial_strings["word"] = text;
		}
		
		internal void updateNow(int cumulativeMilliseconds)
		{
			float deltaSeconds = (now - previous) / 1000.0f;
			update(deltaSeconds);
			previous = now;
		}
		
		internal void update(float deltaSeconds)
		{
			if (isGamePlaying) {
				responseSeconds += deltaSeconds;
				updatePosition(deltaSeconds);
			}
			updateHintVisible();
			metrics.Update(deltaSeconds);
		}
		
		internal void scaleToScreen(float screenWidth)
		{
			scale = screenWidth / width;
		}
		
		/**
		 * Test case:  2015-03 Use Mac. Rosa Zedek expects to read key to change level.
		 * During tutor, clamp word position below help message.
		 * Test case:  2016-06-28 Tutor.  Enter smaller words.  Expect to read letters.  Got overlapped by help.
		 * During tutorial, cannot run out of time.
		 * Test case:  2016-07-23 Tutor.  Neighbor Kristine expects time to enter word.  Got game over.
		 */
		private void clampWordPosition()
		{
			float min = wordWidth - width;
			if (!isTutor() && wordPosition <= min)
			{
				help = "GAME OVER!";
				helpState = "gameOver";
				isGamePlaying = false;
				isContinueVisible = true;
				isNewGameVisible = true;
				metrics.trial_integers["game_over"] = 1;
				metrics.EndTrial();
				metrics.EndSession();
			}
			float max = isTutor() ? -0.4f * wordWidth : 0.0f;
			wordPosition = Mathf.Max(min, Mathf.Min(max, wordPosition));
		}
	
		// Do not tween word position.
		// Test case:  2016-07-23
		// Continue.
		// Next word.
		// Neighbor Kristine expects time to enter.
		// Got word still at the bottom.
		private void updatePosition(float deltaSeconds)
		{
			wordPosition += (deltaSeconds * width * wordWidthPerSecond);
			clampWordPosition();
			wordPositionMin = Mathf.Min(wordPosition, wordPositionMin);
			wordPositionScaled = wordPosition * scale;
			// wordPositionScaled += (nextPosition - wordPositionScaled) * deltaSeconds;
			bool isVerbosePosition = false;
			if (isVerbosePosition) Debug.Log("Model.updatePosition: " + wordPosition);
			updateProgress(deltaSeconds);
		}

		// Scale scrolling to arrive at each checkpoint in the world on each step of normalized progress.
		private void setupProgress()
		{
			progressScale = checkpointInterval / checkpointStep / width;
			progress.SetCheckpointStep(checkpointStep);
		}

		private void updateProgress(float deltaSeconds)
		{
			progressPositionScaled = progressScale * width 
				* progress.NextCreep(performance());
			progressPositionTweened += (progressPositionScaled - progressPositionTweened) * deltaSeconds;
		}
		
		private float outputKnockback = 0.0f;
		
		internal bool mayKnockback()
		{
			return 0 < outputKnockback && 1 <= DataUtil.Length(outputs);
		}
		
		/**
		 * Clamp word to appear on screen.  Test case:  2015-04-18 Complete word.  See next word slide in.
		 */
		private void prepareKnockback(int length, bool complete)
		{
			float perLength =
			0.03f;
			// 0.05;
			// 0.1;
			outputKnockback = perLength * width * length;
			if (complete) {
				outputKnockback *= 3;
			}
			clampWordPosition();
		}
		
		internal bool onOutputHitsWord()
		{
			bool enabled = mayKnockback();
			if (enabled)
			{
				wordPosition += outputKnockback;
				if ("complete" != state)
				{
					//? shuffleNotWord(wordHash, word, readingOrder);
					Deck.ShuffleList(stationIndexes);
				}
				selects = DataUtil.CloneList(word);
				for (int i = 0; i < DataUtil.Length(inputs); i++)
				{
					string letter = inputs[i];
					int selected = selects.IndexOf(letter);
					if (0 <= selected)
					{
						selects[selected] = letter.ToLower();
					}
				}
				outputKnockback = 0;
				wordStateNow = "complete" == state ? "complete" : "hit";
			}
			return enabled;
		}
		
		/**
		 * @param   justPressed	 Filter signature justPressed(letter):Boolean.
		 */
		internal List<string> getPresses(/*<Function>*/IsJustPressed justPressed)
		{
			List<string> presses = new List<string>();
			Dictionary<string, object> letters = new Dictionary<string, object>(){
			}
			;
			for (int i = 0; i < DataUtil.Length(available); i++)
			{
				string letter = available[i];
				if (letters.ContainsKey(letter))
				{
					continue;
				}
				else
				{
					letters[letter] = true;
				}
				if (justPressed(letter))
				{
					presses.Add(letter);
				}
			}
			return presses;
		}
		
		/**
		 * If letter not available, disable typing it.
		 * @return Vector of word indexes.
		 */
		internal List<int> press(List<string> presses)
		{
			Dictionary<string, object> letters = new Dictionary<string, object>(){
			}
			;
			List<int> selectsNow = new List<int>();
			for (int i = 0; i < DataUtil.Length(presses); i++)
			{
				string letter = presses[i];
				if (letters.ContainsKey(letter))
				{
					continue;
				}
				else
				{
					letters[letter] = true;
				}
				int index = available.IndexOf(letter);
				if (0 <= index)
				{
					available.RemoveRange(index, 1);
					inputs.Add(letter);
					int selected = selects.IndexOf(letter);
					if (0 <= selected)
					{
						select(selectsNow, selected, letter);
					}
				}
			}
			return selectsNow;
		}

		private void select(List<int> selectsNow, int selected, string letter)
		{
			selectsNow.Add(selected);
			selects[selected] = letter.ToLower();
			if ("repeat" == helpState)
			{
				helpState = "";
				help = "";
			}
		}

		internal List<int> mouseDown(int selected)
		{
			List<int> selectsNow = new List<int>();
			if (0 <= selected) {
				string letter = word[selected];
				int index = available.IndexOf(letter);
				if (0 <= index) {
					available.RemoveRange(index, 1);
					inputs.Add(letter);
					select(selectsNow, selected, letter);
				}
			}
			return selectsNow;
		}
		
		internal List<int> backspace()
		{
			List<int> selectsNow = new List<int>();
			if (1 <= DataUtil.Length(inputs))
			{
				string letter = DataUtil.Pop(inputs);
				available.Add(letter);
				int selected = selects.LastIndexOf(letter.ToLower());
				if (0 <= selected)
				{
					selectsNow.Add(selected);
					selects[selected] = letter;
				}
			}
			return selectsNow;
		}
	
		private void populateHint(string text)
		{
			hints.Clear();
			isHintVisible = false;
			submitsUntilHintNow = 0;
			hintWord = text;
		}
		
		private void updateHintVisible()
		{
			if (!isGamePlaying) {
				isHintVisible = false;
			}
			else {
				float hintPerformanceMax = // 0.25f;
											// 0.375f;
											0.5f;
				if (performance() <= hintPerformanceMax) {
					isHintVisible = submitsUntilHintNow <= 0;
				}
				else {
					isHintVisible = false;
				}
			}
		}

		internal void hint()
		{
			if (isHintVisible && hints.Count < word.Count) {
				submitsUntilHintNow = submitsUntilHint;
				isHintVisible = false;
				string letter = hintWord.Substring(hints.Count, 1);
				hints.Add(letter);
				metrics.trial_integers["hint_count"]++;
			}
		}

		internal void newGame()
		{
			if (isNewGameVisible)
			{
				if (isVerbose)
				{
					Debug.Log("Model.newGame");
				}
				previousSessionLevel = 0;
				progress.SetLevelNormal(previousSessionLevel);
				trial(levels.parameters[0]);
			}
		}

		/**
		 * When tapping Continue, but not at every next trial, clear inputs.  Animation displays inputs.
		 * Test case:  2016-06-19 Some letters selected.  Game over.  Continue.  Expect no letter selected.  
		 * + Submit full word.  Expect animation.  Got nothing.
		 *
		 * When continue, add trial count.
		 * Test case:  2016-07-23 Tutor.  Game over.  Continue.  Finish tutor trial.  Neighbor Kristine could expect word near top.  Got word near bottom.
		 */
		internal void doContinue()
		{
			if (isContinueVisible) {
				int level = Mathf.Max(progress.GetLevelNormal(), previousSessionLevel);
				if (isVerbose)
				{
					Debug.Log("Model.doContinue: level " + level);
				}
				progress.SetLevelNormal(level);
				trialCount++;
				metrics.StartSession();
				nextTrial();
				DataUtil.Clear(inputs);
			}
		}

		/**
		 * @return animation state.
		 *	  "submit" or "complete":  Word shoots. Test case:  2015-04-18 Anders sees word is a weapon.
		 *	  "submit":  Shuffle letters.  Test case:  2015-04-18 Jennifer wants to shuffle.  Irregular arrangement of letters.  Jennifer feels uncomfortable.
		 * Test case:  2015-04-19 Backspace. Deselect. Submit. Type. Select.
		 * Copy outputs from inputs, even if no submission length.
		 * Test case:  2016-06-25 Submit word.  Submit again.  Expect to see no output.  Saw output.
		 */
		internal string submit()
		{
			string submission = DataUtil.Join(inputs, "");
			bool accepted = false;
			state = "wrong";
			if (1 <= DataUtil.Length(submission))
			{
				if (wordHash.ContainsKey(submission))
				{
					if (repeat.ContainsKey(submission))
					{
						state = "repeat";
						if (levels.index <= 50 && "" == help)
						{
							help = "WORD REPEATED";
							helpState = "repeat";
						}
					}
					else
					{
						if ("repeat" == helpState)
						{
							helpState = "";
							help = "";
						}
						repeat[submission] = true;
						accepted = true;
						scoreUp(submission);
						bool complete = DataUtil.Length(text) == DataUtil.Length(submission);
						prepareKnockback(DataUtil.Length(submission), complete);
						if (complete)
						{
							completes = DataUtil.CloneList(word);
							metrics.EndTrial();
							levelUp();
							if (isVerbose)
							{
								Debug.Log("Model.submit: " + submission 
									+ " " + progress.GetLevelNormal());
							}
							state = "complete";
							trialCount++;
							if (null != onComplete)
							{
								onComplete();
							}
						}
						else
						{
							state = "submit";
							submitsUntilHintNow--;
						}
					}
				}
			}
			outputs = DataUtil.CloneList(inputs);
			if (!accepted) {
				float perWordNotAccepted = -0.1f;
				wordPosition += perWordNotAccepted;
			}
			if (isVerbose) Debug.Log("Model.submit: " + submission + ". Accepted " + accepted);
			DataUtil.Clear(inputs);
			available = DataUtil.CloneList(word);
			selects = DataUtil.CloneList(word);
			return state;
		}
		
		private void scoreUp(string submission)
		{
			points = DataUtil.Length(submission);
			score += points;
		}

		private float performance()
		{
			float bestResponseSeconds = 0.75f * word.Count;
			float worstResponseSeconds = 4.0f * bestResponseSeconds;
			float positionNormal = (width + wordPositionMin) / width;
			float responseRate = (responseSeconds - worstResponseSeconds) / (bestResponseSeconds - worstResponseSeconds);
			responseRate = Mathf.Max(0.0f, Mathf.Min(1.0f, responseRate));
			float performanceNormal = 0.5f * positionNormal + 0.5f * responseRate;
			return performanceNormal;
		}

		private bool updateCheckpoint()
		{
			progress.UpdateCheckpoint();
			if (progress.isCheckpoint) {
				isGamePlaying = false;
				isContinueVisible = true;
				help = "BRILLIANT! YOU REACHED WORD " + progress.level + " OF " + progress.levelMax;
				populateWord("");
				//- metrics.EndSession();
				float checkpoint = progressScale * width * progress.normal;
				if (isVerbose) {
					Debug.Log("Model.updateCheckpoint: " + progress.checkpoint + " progress " + progress.normal + " progressPositionScaled " + progressPositionScaled + " checkpoint " + checkpoint);
				}
			}
			return progress.isCheckpoint;
		}

		private bool isTutor()
		{
			return progress.GetLevelNormal() < tutorLevel 
				&& trialCount < tutorLevel;
		}

		// If next trial starts; otherwise checkpoint.
		internal void nextTrial()
		{
			bool isNow = !updateCheckpoint();
			if (isNow) {
				Dictionary<string, object> level;
				isHudVisible = !isTutor();
				if (isHudVisible) {
					level = progress.Pop(levels.parameters, tutorLevel);
				}
				else {
					level = levels.up();
				}
				trial(level);
			}
		}

		// Level up by response time and worst word position.
		// Test case:  2016-05-21 Jennifer Russ expects to feel challenged.  Got overwhelmed around word 2300 to 2500.
		internal void levelUp()
		{
			if (isTutor()) {
			}
			else {
				progress.Creep(performance());
				metrics.trial_integers["level_up"] = progress.GetLevelNormal() - metrics.trial_integers["level_start"];
			}
		}

		private int previousSessionLevel;

		internal void load(Dictionary<string, object> data)
		{
			if (null != data) {
				if (data.ContainsKey("level")) {
					previousSessionLevel = (int)(data["level"]);
					isContinueVisible = true;
					help = "WORD GARDEN";
					if (isVerbose)
					{
						Debug.Log("Model.load: level " + previousSessionLevel);
					}
				}
				else {
					Debug.Log("Data does not contain level.");
				}
			}
		}

		internal void levelDownMax()
		{
			score = 0;
			progress.Creep(0.0f);
			nextTrial();
			wordPosition = 0.0f;
		}

		private List<int> readingOrder = new List<int>();

		internal void setReadingOrder(List<SceneNodeModel> letterNodes)
		{	
			DataUtil.Clear(readingOrder);
			for (int letter = 0; letter < DataUtil.Length(letterNodes); letter++)
			{
				int index = Toolkit.ParseIndex(letterNodes[letter].name);
				readingOrder.Add(index);
			}
		}
	}
}
