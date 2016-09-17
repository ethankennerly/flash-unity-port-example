using UnityEngine;
using System.Collections.Generic;

using /*<com>*/Finegamedesign.Utils/*<Model>*/;
namespace /*<com>*/Finegamedesign.Anagram
{
	[System.Serializable]
	public sealed class AnagramModel
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

		private static void ShuffleNotWord(Dictionary<string, object> wordHash, List<string> letters, List<int> readingOrder, int attemptMax = 16)
		{
			for (int attempt = 0; attempt < attemptMax; attempt++)
			{
				Deck.ShuffleList(letters);
				if (!ContainsWord(wordHash, letters, readingOrder)) {
					break;
				}
			}
		}
		
		public Journal journal = new Journal();
		public bool isHelpStateChange;
		public bool isNewGameVisible = false;
		public string help = "";
		public string helpState;
		public string helpStateBefore;
		public string state;
		public string text;

		internal string title;
		internal List<int> stationIndexes;
		internal int letterMax = 10;
		internal List<string> inputs = new List<string>();
		//
		// From letter graphic.
		// 
		internal float letterWidth = 42.0f;
		internal float wordWidth = 
									// 160.0f;
									420.0f; 
		internal /*<Function>*/ActionDelegate onComplete;
		internal Dictionary<string, object> wordHash;
		internal Levels levels = new Levels();
		internal List<string> completes = new List<string>();
		internal List<string> hints = new List<string>();
		internal List<string> outputs = new List<string>();
		internal List<string> word;
		internal Metrics metrics = new Metrics();
		internal Progress progress = new Progress();
		internal bool isContinueVisible = false;
		internal bool isGamePlaying = false;
		internal bool isHintVisible = false;
		internal bool isHudVisible = false;
		internal bool isInstant = false;
		internal bool isPaused = false;
		internal bool isSaveNow = false;
		internal delegate /*<object>*/void ActionDelegate();
		internal delegate bool IsJustPressed(string letter);
		internal float checkpointStep = 0.1f;
		internal float nextProgress = 0.0f;
		internal float progressPositionScaled = 0.0f;
		internal float progressPositionTweened = 0.0f;
		internal float progressScale;
		internal float scale = 1.0f;
		internal float width = 720;
		internal float wordPosition = 0.0f;
		internal float wordPositionScaled = 0.0f;
		internal int points = 0;
		internal int score = 0;
		internal int submitsUntilHint = 1; // 3;
		internal int submitsUntilHintNow;
		internal int tutorLevel = 0;
		internal string wordStateNow;
		private Dictionary<string, object> repeat = new Dictionary<string, object>(){ } ;
		private List<string> available;
		private List<string> selects;
		private bool isVerbose = true;
		private float checkpointInterval = -16.0f; 
		private float responseSeconds;
		private float wordPositionMin;
		private float wordWidthPerSecond;
		private int now = 0;
		private int previous = 0;
		private int previousSessionLevel;
		private int trialCount;
		private string hintWord;
		
		public void Setup()
		{
			state = null;
			SetupProgress();
			tutorLevel = 3;
			trialCount = 0;
			isNewGameVisible = true;
			PopulateWord("");
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
		
		private void PopulateWord(string text)
		{
			this.text = text;
			available = DataUtil.Split(text, "");
			word = DataUtil.CloneList(available);
			PopulateHint(text);
			stationIndexes = new List<int>();
			for (int index = 0; index < DataUtil.Length(word); index++)
			{
				stationIndexes.Add(index);
			}
		}

		//
		// In tutor, word speed is half.
		// Otherwise HUD is visible.
		// Test case:  2016-07-31 Continue. Game over. New game.
		// Expect HUD goes away.  Got HUD remains.
		// Complete tutorial trials.  Expect HUD slides into view.
		// At start of trial, set help state to none.  Was null.
		// Test case:  2016-08-20 Complete tutorial.  Expect help disappears. 
		internal void StartTrial(Dictionary<string, object> parameters)
		{
			isHudVisible = !IsTutor();
			isGamePlaying = true;
			isContinueVisible = false;
			isNewGameVisible = false;
			responseSeconds = 0.0f;
			wordPosition = 0.0f;
			wordPositionMin = 0.0f;
			help = "";
			helpState = "none";
			wordWidthPerSecond = IsTutor() ? -0.005f : -0.01f;
			if (parameters.ContainsKey("text")) {
				text = (string)parameters["text"];
			}
			if (parameters.ContainsKey("help")) {
				help = (string)parameters["help"];
				helpState = "tutor";
			}
			if (parameters.ContainsKey("wordWidthPerSecond")) {
				wordWidthPerSecond = (float)parameters["wordWidthPerSecond"];
			}
			if (parameters.ContainsKey("wordPosition")) {
				wordPosition = (float)parameters["wordPosition"];
			}
			PopulateWord(text);
			if ("" == help)
			{
				//? ShuffleNotWord(wordHash, word, readingOrder);
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
			state = "trial";
			if (isVerbose) 
			{
				DebugUtil.Log("AnagramModel.StartTrial: word[0]: <" + word[0] + ">" 
					+ " level " + progress.GetLevelNormal());
			}
			metrics.StartTrial();
			metrics.trial_integers["game_over"] = 0;
			metrics.trial_integers["hint_count"] = 0;
			metrics.trial_integers["level_start"] = progress.GetLevelNormal();
			metrics.trial_integers["level_up"] = 0;
			metrics.trial_strings["word"] = text;
		}
		
		internal void UpdateNow(int cumulativeMilliseconds)
		{
			float deltaSeconds = (now - previous) / 1000.0f;
			Update(deltaSeconds);
			previous = now;
		}
		
		public void Update(float deltaSeconds)
		{
			if (isGamePlaying) {
				responseSeconds += deltaSeconds;
				UpdatePosition(deltaSeconds);
			}
			DataUtil.Clear(selectsNow);
			DataUtil.Clear(backspacesNow);
			UpdateHintVisible();
			metrics.Update(deltaSeconds);
			journal.Update(deltaSeconds);
			UpdateCommand(journal.commandNow);
			isHelpStateChange = helpStateBefore != helpState;
			helpStateBefore = helpState;
		}

		private void UpdateCommand(string command)
		{
			if (null == command)
			{
			}
			else if ("submit" == command)
			{
				Submit();
			}
			else if ("newGame" == command)
			{
				NewGame();
			}
			else if ("continue" == command)
			{
				ContinueGame();
			}
			else if ("delete" == command)
			{
				Backspace();
			}
			else if ("hint" == command)
			{
				Hint();
			}
			else if ("trialComplete" == command)
			{
				TrialComplete();
			}
			else if (command.IndexOf("select_") == 0)
			{
				int index = StringUtil.ParseIndex(command);
				MouseDown(index);
			}
			else
			{
				throw new System.InvalidOperationException("Did not expect command <" + command + ">");
			}
		}
		
		internal void ScaleToScreen(float screenWidth)
		{
			scale = screenWidth / width;
		}
		
		//
		// Test case:  2015-03 Use Mac. Rosa Zedek expects to read key to change level.
		// During tutor, clamp word position below help message.
		// Test case:  2016-06-28 Tutor.  Enter smaller words.  Expect to read letters.  Got overlapped by help.
		// During tutorial, cannot run out of time.
		// Test case:  2016-07-23 Tutor.  Neighbor Kristine expects time to enter word.  Got game over.
		// 
		private void ClampWordPosition()
		{
			float min = wordWidth - width;
			if (!IsTutor() && wordPosition <= min)
			{
				GameOver();
			}
			float max = IsTutor() ? -0.4f * wordWidth : 0.0f;
			wordPosition = Mathf.Max(min, Mathf.Min(max, wordPosition));
		}
	
		internal void GameOver()
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
	
		internal void Pause(bool isInstant = false)
		{
			isPaused = true;
			this.isInstant = isInstant;
			help = "PAUSED";
			helpState = "paused";
			isGamePlaying = false;
			isContinueVisible = true;
			isNewGameVisible = true;
		}

		// On resume, set help state to "unpaused" instead of null.
		// Test case:  2016-08-20 Unpause.  Expect help disappears.
		internal void Resume()
		{
			isPaused = false;
			isInstant = false;
			help = "";
			helpState = "unpaused";
			isGamePlaying = true;
			isContinueVisible = false;
			isNewGameVisible = false;
		}

		// Do not tween word position.
		// Test case:  2016-07-23
		// Continue.
		// Next word.
		// Neighbor Kristine expects time to enter.
		// Got word still at the bottom.
		private void UpdatePosition(float deltaSeconds)
		{
			wordPosition += (deltaSeconds * width * wordWidthPerSecond);
			ClampWordPosition();
			wordPositionMin = Mathf.Min(wordPosition, wordPositionMin);
			wordPositionScaled = wordPosition * scale;
			// wordPositionScaled += (nextPosition - wordPositionScaled) * deltaSeconds;
			bool isVerbosePosition = false;
			if (isVerbosePosition) DebugUtil.Log("AnagramModel.UpdatePosition: " + wordPosition);
			UpdateProgress(deltaSeconds);
		}

		// Scale scrolling to arrive at each checkpoint in the world on each step of normalized progress.
		private void SetupProgress()
		{
			progressScale = checkpointInterval / checkpointStep / width;
			progress.SetCheckpointStep(checkpointStep);
			progress.levelMax = levels.Count();
		}

		private void UpdateProgress(float deltaSeconds)
		{
			nextProgress = progress.NextCreep(Performance());
			progressPositionScaled = progressScale * width 
				* nextProgress;
			progressPositionTweened += (progressPositionScaled - progressPositionTweened) * deltaSeconds;
		}
		
		private float outputKnockback = 0.0f;
		
		internal bool MayKnockback()
		{
			return 0 < outputKnockback && 1 <= DataUtil.Length(outputs);
		}
		
		//
		// Clamp word to appear on screen.  Test case:  2015-04-18 Complete word.  See next word slide in.
		// 
		private void PrepareKnockback(int length, bool complete)
		{
			float perLength =
			0.03f;
			// 0.05;
			// 0.1;
			outputKnockback = perLength * width * length;
			if (complete) {
				outputKnockback *= 3;
			}
			ClampWordPosition();
		}
		
		internal bool OnOutputHitsWord()
		{
			bool enabled = MayKnockback();
			if (enabled)
			{
				wordPosition += outputKnockback;
				if ("complete" != state)
				{
					//? ShuffleNotWord(wordHash, word, readingOrder);
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
		
		//
		// @param   justPressed	 Filter signature justPressed(letter):Boolean.
		// 
		internal List<string> GetPresses(/*<Function>*/IsJustPressed justPressed)
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
		
		//
		// If letter not available, disable typing it.
		// @return Vector of word indexes.
		// 
		internal List<int> Press(List<string> presses)
		{
			Dictionary<string, object> letters = new Dictionary<string, object>(){
			}
			;
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
						Select(selected, letter);
					}
				}
			}
			return selectsNow;
		}

		internal List<int> selectsNow = new List<int>();

		private void Select(int selected, string letter)
		{
			selectsNow.Add(selected);
			selects[selected] = letter.ToLower();
			if ("repeat" == helpState)
			{
				helpState = "";
				help = "";
			}
			journal.Record("select_" + selected.ToString());
		}

		internal List<int> MouseDown(int selected)
		{
			if (0 <= selected && selected < DataUtil.Length(word)) {
				string letter = word[selected];
				int index = available.IndexOf(letter);
				if (0 <= index) {
					available.RemoveRange(index, 1);
					inputs.Add(letter);
					Select(selected, letter);
				}
			}
			return selectsNow;
		}
	
		internal List<int> backspacesNow = new List<int>();

		internal List<int> Backspace()
		{
			if (1 <= DataUtil.Length(inputs))
			{
				string letter = DataUtil.Pop(inputs);
				available.Add(letter);
				int selected = selects.LastIndexOf(letter.ToLower());
				if (0 <= selected)
				{
					backspacesNow.Add(selected);
					selects[selected] = letter;
				}
			}
			journal.Record("delete");
			return backspacesNow;
		}
	
		private void PopulateHint(string text)
		{
			hints.Clear();
			isHintVisible = false;
			submitsUntilHintNow = 0;
			hintWord = text;
		}
		
		private void UpdateHintVisible()
		{
			if (!isGamePlaying) {
				isHintVisible = false;
			}
			else {
				float hintPerformanceMax = // 0.25f;
											// 0.375f;
											0.5f;
				if (Performance() <= hintPerformanceMax) {
					isHintVisible = submitsUntilHintNow <= 0;
				}
				else {
					isHintVisible = false;
				}
			}
		}

		internal void Hint()
		{
			if (isHintVisible && hints.Count < word.Count) {
				submitsUntilHintNow = submitsUntilHint;
				isHintVisible = false;
				string letter = hintWord.Substring(hints.Count, 1);
				hints.Add(letter);
				metrics.trial_integers["hint_count"]++;
			}
			journal.Record("hint");
		}

		public void NewGame()
		{
			if (isNewGameVisible)
			{
				if (isVerbose)
				{
					DebugUtil.Log("AnagramModel.NewGame");
				}
				previousSessionLevel = 0;
				progress.SetLevelNormal(previousSessionLevel);
				StartTutorial();
			}
			journal.Record("newGame");
		}

		private void StartTutorial()
		{
			trialCount = 0;
			StartTrial(levels.parameters[0]);
		}

		//
		// When tapping Continue, but not at every next trial, clear inputs.  Animation displays inputs.
		// Test case:  2016-06-19 Some letters selected.  Game over.  Continue.  Expect no letter selected.  
		// + Submit full word.  Expect animation.  Got nothing.
		// 
		// When continue, add trial count.
		// Test case:  2016-07-23 Tutor.  Game over.  Continue.  Finish tutor trial.  Neighbor Kristine could expect word near top.  Got word near bottom.
		// 
		internal void ContinueGame(int level = -1)
		{
			if (isContinueVisible) {
				if (isPaused)
				{
					Resume();
					return;
				}
				if (level <= -1)
				{
					level = Mathf.Max(progress.GetLevelNormal(), previousSessionLevel);
				}
				if (isVerbose)
				{
					DebugUtil.Log("AnagramModel.ContinueGame: level " + level);
				}
				progress.SetLevelNormal(level);
				SelectLevel(progress.level);
			}
			journal.Record("continue");
		}

		// Not recorded in journal.
		internal void SelectLevel(int contentIndex)
		{
			progress.level = contentIndex;
			trialCount++;
			metrics.StartSession();
			DataUtil.Clear(inputs);
			help = "";
			helpState = null;
			if (IsTutor())
			{
				StartTutorial();
			}
			else {
				NextTrial();
			}
		}

		//
		// @return animation state.
		// 	  "submit" or "complete":  Word shoots. Test case:  2015-04-18 Anders sees word is a weapon.
		// 	  "submit":  Shuffle letters.  Test case:  2015-04-18 Jennifer wants to shuffle.  Irregular arrangement of letters.  Jennifer feels uncomfortable.
		// Test case:  2015-04-19 Backspace. Deselect. Submit. Type. Select.
		// Copy outputs from inputs, even if no submission length.
		// Test case:  2016-06-25 Submit word.  Submit again.  Expect to see no output.  Saw output.
		// 
		public string Submit()
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
						ScoreUp(submission);
						bool complete = DataUtil.Length(text) == DataUtil.Length(submission);
						PrepareKnockback(DataUtil.Length(submission), complete);
						if (complete)
						{
							completes = DataUtil.CloneList(word);
							metrics.EndTrial();
							LevelUp();
							if (isVerbose)
							{
								DebugUtil.Log("AnagramModel.submit: " + submission 
									+ " " + progress.GetLevelNormal());
							}
							state = "complete";
							isSaveNow = true;
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
			if (isVerbose) DebugUtil.Log("AnagramModel.submit: " + submission + ". Accepted " + accepted);
			DataUtil.Clear(inputs);
			available = DataUtil.CloneList(word);
			selects = DataUtil.CloneList(word);
			journal.Record("submit");
			return state;
		}
		
		private void ScoreUp(string submission)
		{
			points = DataUtil.Length(submission);
			score += points;
		}

		private float Performance()
		{
			float bestResponseSeconds = 0.75f * word.Count;
			float worstResponseSeconds = 4.0f * bestResponseSeconds;
			float positionNormal = (width + wordPositionMin) / width;
			float responseRate = (responseSeconds - worstResponseSeconds) / (bestResponseSeconds - worstResponseSeconds);
			responseRate = Mathf.Max(0.0f, Mathf.Min(1.0f, responseRate));
			float performanceNormal = 0.5f * positionNormal + 0.5f * responseRate;
			return performanceNormal;
		}

		private bool UpdateCheckpoint()
		{
			progress.UpdateCheckpoint();
			if (progress.isCheckpoint) {
				isGamePlaying = false;
				isContinueVisible = true;
				help = "BRILLIANT! YOU REACHED WORD " + progress.level + " OF " + progress.levelMax;
				helpState = "checkpoint";
				PopulateWord("");
				//- metrics.EndSession();
				if (isVerbose) {
					float checkpoint = progressScale * width * progress.normal;
					DebugUtil.Log("AnagramModel.UpdateCheckpoint: " + progress.checkpoint 
						+ " progress " + progress.normal 
						+ " progressPositionScaled " + progressPositionScaled 
						+ " checkpoint " + checkpoint);
				}
			}
			return progress.isCheckpoint;
		}

		private bool IsTutor()
		{
			return progress.GetLevelNormal() < tutorLevel 
				&& trialCount < tutorLevel;
		}

		internal void TrialComplete()
		{
			NextTrial();
			journal.Record("trialComplete");
		}

		// If next trial starts; otherwise checkpoint.
		// Update if HUD is visible before selecting the next level.
		// Test case:  2016-08-14 Continue. First word.  Expect to read about 3000 words.  Got 1000 words.
		internal void NextTrial()
		{
			bool isNow = !UpdateCheckpoint();
			if (isNow) {
				isHudVisible = !IsTutor();
				Dictionary<string, object> level;
				if (isHudVisible) {
					level = progress.Pop(levels.parameters, tutorLevel);
				}
				else {
					level = levels.Up();
				}
				StartTrial(level);
			}
		}

		// Level up by response time and worst word position.
		// Test case:  2016-05-21 Jennifer Russ expects to feel challenged.  Got overwhelmed around word 2300 to 2500.
		internal void LevelUp()
		{
			if (IsTutor()) {
			}
			else {
				progress.Creep(Performance());
				metrics.trial_integers["level_up"] = progress.GetLevelNormal() - metrics.trial_integers["level_start"];
			}
		}

		internal void Load(Dictionary<string, object> data)
		{
			if (null != data) {
				if (data.ContainsKey("level")) {
					previousSessionLevel = (int)(data["level"]);
					progress.SetLevelNormal(previousSessionLevel);
					isContinueVisible = true;
					help = title;
					helpState = "title";
					if (isVerbose)
					{
						DebugUtil.Log("AnagramModel.Load: level " + previousSessionLevel);
					}
				}
				else {
					DebugUtil.Log("Data does not contain level.");
				}
			}
		}

		internal void LevelDownMax()
		{
			score = 0;
			progress.Creep(0.0f);
			NextTrial();
			wordPosition = 0.0f;
		}

		private List<int> readingOrder = new List<int>();

		internal void SetReadingOrder(List<SceneNodeModel> letterNodes)
		{	
			DataUtil.Clear(readingOrder);
			for (int letter = 0; letter < DataUtil.Length(letterNodes); letter++)
			{
				int index = StringUtil.ParseIndex(letterNodes[letter].name);
				readingOrder.Add(index);
			}
		}
	}
}
