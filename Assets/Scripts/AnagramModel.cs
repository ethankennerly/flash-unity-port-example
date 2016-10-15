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
		
		public Hint hint = new Hint();
		public Journal journal = new Journal();
		public bool isContinueVisible = false;
		public bool isNewGameVisible = false;
		public Watcher<string> helpState = Watcher<string>.Create("");
		public Watcher<string> helpTextNow = Watcher<string>.Create("");
		public string helpStateNow = null;
		public Levels levels = new Levels();
		public Progress progress = new Progress();
		public string state;
		public string text;
		public int trialCount;
		public int trialPeriod = 10;
		public int tutorLevel = 3;

		// Select letters:
		public LetterSelectModel select = new LetterSelectModel();
		internal int letterMax = 10;
		internal delegate bool IsJustPressed(string letter);

		internal List<string> completes = new List<string>();
		internal List<string> hints = new List<string>();
		internal List<string> outputs = new List<string>();

		internal string title;
		internal List<int> stationIndexes;
		// From letter graphic.
		internal float wordWidth = 
									// 160.0f;
									420.0f; 
		internal delegate /*<object>*/void ActionDelegate();
		internal /*<Function>*/ActionDelegate onComplete;
		internal Dictionary<string, object> wordHash;
		internal Metrics metrics = new Metrics();
		internal bool isGamePlaying = false;
		internal bool isHintVisible = false;
		internal bool isHudVisible = false;
		internal bool isInstant = false;
		internal bool isPaused = false;
		internal bool isSaveNow = false;
		internal float checkpointStep = 0.1f;
		internal float scale = 1.0f;
		internal float width = 720.0f;
		internal float wordPosition = 0.0f;
		internal float wordPositionScaled = 0.0f;
		internal int points = 0;
		internal int score = 0;
		internal int submitsUntilHint = 1; // 3;
		internal int submitsUntilHintNow;
		internal Watcher<string> wordState = new Watcher<string>();

		private Dictionary<string, object> repeat = new Dictionary<string, object>(){ } ;
		private bool isVerbose = true;
		private float responseSeconds;
		private float wordPositionMin;
		private float wordWidthPerSecond;
		private int now = 0;
		private int previous = 0;
		private int previousSessionLevel;
		
		public void Setup()
		{
			state = null;
			SetupProgress();
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
		
		public void PopulateWord(string text)
		{
			select.PopulateWord(text);
			this.text = text;
			PopulateHint(text);
			stationIndexes = new List<int>();
			for (int index = 0; index < DataUtil.Length(select.word); index++)
			{
				stationIndexes.Add(index);
			}
		}

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
			helpTextNow.next = "";
			helpState.next = "none";
			wordWidthPerSecond = IsTutor() ? -0.005f : -0.01f;
			if (parameters.ContainsKey("text")) {
				text = (string)parameters["text"];
			}
			if (parameters.ContainsKey("help")) {
				helpTextNow.next = (string)parameters["help"];
				helpState.next = "tutor";
			}
			if (parameters.ContainsKey("wordWidthPerSecond")) {
				wordWidthPerSecond = (float)parameters["wordWidthPerSecond"];
			}
			if (parameters.ContainsKey("wordPosition")) {
				wordPosition = (float)parameters["wordPosition"];
			}
			PopulateWord(text);
			if ("" == helpTextNow.next)
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
			repeat = new Dictionary<string, object>(){};
			wordState.next = "begin";
			state = "trial";
			if (isVerbose) 
			{
				DebugUtil.Log("AnagramModel.StartTrial: word[0]: <" + select.word[0] + ">" 
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
				UpdateCheckpoint();
			}
			select.selectedIndexes.Update();
			hint.isVisible = isGamePlaying;
			metrics.Update(deltaSeconds);
			journal.Update(deltaSeconds);
			UpdateCommand(journal.commandNow);
			UpdateHelp();
			wordState.Update(wordState.next);
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
				select.Toggle(index);
				Select(index);
			}
			else
			{
				throw new System.InvalidOperationException("Did not expect command <" + command + ">");
			}
		}
		
		private void UpdateHelp()
		{
			helpState.Update(helpState.next);
			helpTextNow.Update(helpTextNow.next);
			if (helpState.IsChange() && helpTextNow.IsChange()) {
				helpStateNow = "" == helpTextNow.next ? "endNow" 
					: isInstant ? "instantNow" : "beginNow";
			}
			else
			{
				helpTextNow.next = null;
				helpStateNow = null;
			}
		}

		internal void ScaleToScreen(float screenWidth)
		{
			scale = screenWidth / width;
		}
		
		// Test case:  2015-03 Use Mac. Rosa Zedek expects to read key to change level.
		// During tutor, clamp word position below help message.
		// Test case:  2016-06-28 Tutor.  Enter smaller words.  Expect to read letters.  Got overlapped by help.
		// During tutorial, cannot run out of time.
		// Test case:  2016-07-23 Tutor.  Neighbor Kristine expects time to enter word.  Got game over.
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
			helpTextNow.next = "GAME OVER!";
			helpState.next = "gameOver";
			isGamePlaying = false;
			isContinueVisible = true;
			isNewGameVisible = true;
			metrics.trial_integers["game_over"] = 1;
			metrics.EndTrial();
			metrics.EndSession();
		}
	
		internal void Pause(bool isInstantNow = false)
		{
			isPaused = true;
			isInstant = isInstantNow;
			helpTextNow.next = "PAUSED";
			helpState.next = "paused";
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
			helpTextNow.next = "";
			helpState.next = "unpaused";
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
			bool isVerbosePosition = false;
			if (isVerbosePosition) DebugUtil.Log("AnagramModel.UpdatePosition: " + wordPosition);
		}

		// Scale scrolling to arrive at each checkpoint in the world on each step of normalized progress.
		// More normalized levels than actual.
		// Test case:  2016-09-18 Word 267.  Save level.
		// Load level.  Expect exactly word 267.  Got sometimes rounded to another number.
		private void SetupProgress()
		{
			progress.SetCheckpointStep(checkpointStep);
			progress.levelNormalMax = 10000;
			progress.SetupIndexes(levels.Count(), tutorLevel);
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
				outputKnockback = 0;
				wordState.next = "complete" == state ? "complete" : "hit";
			}
			return enabled;
		}
	
		private void PopulateHint(string text)
		{
			DataUtil.Clear(hint.reveals);
			hint.answer = text;
		}

		internal void Hint()
		{
			if (hint.Select()) {
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

		// When start tutorial, reset level to 0.
		// Test case:  2016-09-17 Play a session.
		// Next session.  Select level 1.
		// Expect word 1.  Got word 640.
		// When start tutorial reset available words.
		//
		// Reset tutorial levels back to 0.
		// XXX Would be cleaner to consolidate level indexes.
		// Test case:  2016-10-09 Select level 1.  Complete 2 words.  Pause.  Select 1.  
		// Complete 1 word. Expect 2nd tutorial word.  
		// Got second trial is not tutorial but no HUD.
		private void StartTutorial()
		{
			SetupProgress();
			trialCount = 0;
			progress.level = 0;
			levels.index = 0;
			progress.SetLevelNormal(0);
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
		// Set level and normal to selected content.
		// Test case:  2016-09-17 Play a session.
		// Select level 10.
		// Expect word 10.  Got word 640.
		internal void SelectLevel(int contentIndex)
		{
			progress.level = contentIndex;
			progress.normal = contentIndex / (float)progress.levelMax;
			trialCount = 0;
			metrics.StartSession();
			DataUtil.Clear(select.inputs);
			helpTextNow.next = "";
			helpState.next = "";
			Resume();
			if (IsTutor())
			{
				StartTutorial();
			}
			else {
				StartTrial(levels.parameters[progress.level]);
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
			string submission = DataUtil.Join(select.inputs, "");
			bool accepted = false;
			state = "wrong";
			DataUtil.Clear(select.selectedIndexes.selects);
			if (1 <= DataUtil.Length(submission))
			{
				if (wordHash.ContainsKey(submission))
				{
					if (repeat.ContainsKey(submission))
					{
						state = "repeat";
						if (IsHelpRepeat())
						{
							helpTextNow.next = "WORD REPEATED";
							helpState.next = "repeat";
						}
						if (isVerbose)
						{
							DebugUtil.Log("AnagramModel.Submit: " + state);
						}
					}
					else
					{
						if ("repeat" == helpState.next)
						{
							helpState.next = "";
							helpTextNow.next = "";
						}
						repeat[submission] = true;
						accepted = true;
						ScoreUp(submission);
						bool complete = DataUtil.Length(text) == DataUtil.Length(submission);
						PrepareKnockback(DataUtil.Length(submission), complete);
						if (complete)
						{
							completes = DataUtil.CloneList(select.word);
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
			outputs = DataUtil.CloneList(select.inputs);
			if (!accepted) {
				float perWordNotAccepted = -0.1f;
				wordPosition += perWordNotAccepted;
			}
			if (isVerbose) DebugUtil.Log("AnagramModel.submit: " + submission + ". Accepted " + accepted);
			DataUtil.Clear(select.inputs);
			journal.Record("submit");
			return state;
		}
	
		public bool IsHelpRepeat()
		{
			return levels.index <= 50;
		}
		
		private void ScoreUp(string submission)
		{
			points = DataUtil.Length(submission);
			score += points;
		}

		private float Performance()
		{
			float bestResponseSeconds = 0.75f * select.word.Count;
			float worstResponseSeconds = 4.0f * bestResponseSeconds;
			float positionNormal = (width + wordPositionMin) / width;
			float responseRate = (responseSeconds - worstResponseSeconds) / (bestResponseSeconds - worstResponseSeconds);
			responseRate = Mathf.Max(0.0f, Mathf.Min(1.0f, responseRate));
			float performanceNormal = 0.5f * positionNormal + 0.5f * responseRate;
			return performanceNormal;
		}

		// 2016-09-18 Jennifer Russ could expect break every 10th word cleared saying ten words cleared. (2016-09-23 +Kelsey Kerlan)
		public bool UpdateTrialCycleCheckpoint()
		{
			bool isNow = IsTrialCycleNow();
			if (isNow)
			{
				string text = "BRILLIANT! YOU REACHED WORD " + progress.level + " OF " + progress.levelMax;
				ShowCheckpoint(text);
			}
			return isNow;
		}

		public bool IsTrialCycleNow()
		{
			return 1 < trialCount && 0 == (trialCount % trialPeriod);
		}

		private void ShowCheckpoint(string text)
		{
			isGamePlaying = false;
			isContinueVisible = true;
			helpState.next = "checkpoint";
			helpTextNow.next = text;
			PopulateWord("");
			//- metrics.EndSession();
			if (isVerbose) {
				DebugUtil.Log("AnagramModel.ShowCheckpoint: " + progress.checkpoint 
					+ " progress " + progress.normal );
			}
		}

		private bool UpdateCheckpoint()
		{
			progress.UpdateCheckpoint();
			if (progress.isCheckpoint) {
				hint.count += hint.countPerCheckpoint;
				string text = "OUTSTANDING! YOU EARNED " 
					+ hint.countPerCheckpoint + " HINTS!";
				ShowCheckpoint(text);
			}
			return progress.isCheckpoint;
		}

		// Check progress index versus tutor level.
		// Test case:  2016-09-18 Select level 2.
		// Expect restart level 1 with words and help both legible.
		// Got level 1 with help overlapping words.
		public bool IsTutor()
		{
			return progress.level < tutorLevel 
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
			bool isNow = ! // UpdateCheckpoint();
					UpdateTrialCycleCheckpoint();
			if (isNow) {
				isHudVisible = !IsTutor();
				Dictionary<string, object> level;
				if (isHudVisible) {
					int index = progress.PopIndex();
					level = levels.parameters[index];
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
			if (!IsTutor()) {
				progress.Creep(Performance());
				metrics.trial_integers["level_up"] = progress.GetLevelNormal() - metrics.trial_integers["level_start"];
			}
		}

		internal void Load(Dictionary<string, object> data)
		{
			if (null != data) {
				LoadLevel(data);
				LoadHint(data);
			}
		}

		internal void LoadLevel(Dictionary<string, object> data)
		{
			if (data.ContainsKey("level")) {
				previousSessionLevel = (int)(data["level"]);
				progress.SetLevelNormalUnlocked(previousSessionLevel);
				isContinueVisible = true;
				helpTextNow.next = title;
				helpState.next = "title";
				if (isVerbose)
				{
					DebugUtil.Log("AnagramModel.Load: level " + previousSessionLevel);
				}
			}
			else {
				DebugUtil.Log("Data does not contain level.");
			}
		}

		internal void LoadHint(Dictionary<string, object> data)
		{
			if (data.ContainsKey("hint")) {
				hint.count = (int)(data["hint"]);
			}
			if (data.ContainsKey("cents")) {
				hint.cents = (int)(data["cents"]);
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

		// Select letters:

		// Test case:  2016-09-18 Jennifer Russ:
		// Tap selected letter.  Expects deselects letter. (2016-09-23 +Kelsey Kerlan).
		// 	Example:  City of Words.
		// Level 106:  LIDE.  Tap I, D, E, D.  Expect only "I" is selected.
		private void Select(int selected)
		{
			if (selected <= -1)
			{
				return;
			}
			if ("repeat" == helpState.previous)
			{
				helpState.next = "";
				helpTextNow.next = "";
			}
			journal.Record("select_" + selected.ToString());
			if (isVerbose)
			{
				DebugUtil.Log("AnagramModel.Select: " + selected);
			}
		}

		public void UpdateSelect(string addInput, int toggleIndex)
		{
			select.Add(addInput);
			select.Toggle(toggleIndex);
			for (int index = 0; index < DataUtil.Length(select.selectedIndexes.selectsNow); index++)
			{
				Select(select.selectedIndexes.selectsNow[index]);
			}
		}
	
		internal void Backspace()
		{
			journal.Record("delete");
		}
	}
}
