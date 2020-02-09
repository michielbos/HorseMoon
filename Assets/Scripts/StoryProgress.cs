using System;
using System.Collections.Generic;
using UnityEngine;
using HorseMoon.Speech;
using HorseMoon.Objects;

namespace HorseMoon
{
	public class StoryProgress : SingletonMonoBehaviour<StoryProgress>
	{
		[Serializable]
		public class StoryData {
			public string[] varNames;
			public string[] values;
		}

		private Dictionary<string, string> variables = new Dictionary<string, string>();

		private void Start()
		{
			Apply();
		}

		public void Set(string varName, object value) {
			variables[varName] = value.ToString();
		}

		public void Clear(string varName) {
			variables.Remove(varName);
		}

		public string GetString(string varName)
		{
			if (variables.ContainsKey(varName))
				return variables[varName];
			return ""; // This should actually throw an exception...
		}

		public int GetInt(string varName)
		{
			if (variables.ContainsKey(varName))
				return int.Parse(variables[varName]);
			return 0; // This should actually throw an exception...
		}

		public bool GetBool(string varName)
		{
			if (variables.ContainsKey(varName))
				return bool.Parse(variables[varName]);
			return false; // This should actually throw an exception...
		}

		public void OnDayPassed()
		{
			SpeechUI.Instance.Behavior.variableStorage.SetValue("$TTSpokeToday", false);

			if (GetInt("CarrotDemand") == 1 && GetInt("CarrotDemandShip") >= 15)
			{
				Set("CarrotDemand", 2);
				Set("UnlockedEggplantSeeds", 1);
			}

			CheckUnlock("UnlockedWatermelonSeeds");
			CheckUnlock("UnlockedBlueberrySeeds");
			CheckUnlock("UnlockedEggplantSeeds");
			CheckUnlock("UnlockedGrapeSeeds");
			CheckUnlock("UnlockedPumpkinSeeds");

			FindObjectOfType<Till>().PickTodayNode();
		}

		private void CheckUnlock(string varName)
		{
			int unlockProgress = GetInt(varName);
			if (unlockProgress == 1)
				Set(varName, 2);
		}

		public StoryData GetStoryData()
		{
			string[] varNames = new string[variables.Keys.Count];
			variables.Keys.CopyTo(varNames, 0);
			string[] values = new string[variables.Values.Count];
			variables.Values.CopyTo(values, 0);
			return new StoryData { varNames = varNames, values = values };
		}

		public void LoadStoryData(StoryData data)
		{
			variables.Clear();
			SpeechUI.Instance.Behavior.variableStorage.Clear();

			for (int i = 0; i < data.varNames.Length; i++)
				variables.Add(data.varNames[i], data.values[i]);

			Apply();
		}

		private void Apply()
		{
			// There are some special cases... -->
			TimeController.Instance.Day = Mathf.Max(GetInt("Day"), 1);

			SpeechUI.Instance.Behavior.variableStorage.SetValue("$TTSpokeToday", false);

			if (GetBool("Nicknamed")) {
				SpeechUI.Instance.Behavior.variableStorage.SetValue("$tenderSatchelNickname", "Satch");
				SpeechUI.Instance.Behavior.variableStorage.SetValue("$satchelTenderNickname", "Tillie");
			} else {
				SpeechUI.Instance.Behavior.variableStorage.SetValue("$tenderSatchelNickname", "Satchel");
				SpeechUI.Instance.Behavior.variableStorage.SetValue("$satchelTenderNickname", "Tender Till");
			}

			if (GetInt("WellStory") > 1)
				FindObjectOfType<BrokenWell>()?.Repair();

			if (GetBool("BridgeRepaired"))
				FindObjectOfType<BrokenBridge>()?.Repair();

			// Please respond! -->
			FindObjectOfType<Till>().PickTodayNode();
		}
	}
}
