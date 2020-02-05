using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorseMoon.Speech;

namespace HorseMoon
{
	public class StoryProgress : SingletonMonoBehaviour<StoryProgress>
	{
		private Dictionary<string, string> data = new Dictionary<string, string>();

		private void Start()
		{
			SpeechUI.Instance.Behavior.variableStorage.SetValue("$tenderSatchelNickname", "Satchel");
			SpeechUI.Instance.Behavior.variableStorage.SetValue("$satchelTenderNickname", "Tender Till");
		}

		public void Set(string varName, object value) {
			data[varName] = value.ToString();
		}

		public void Clear(string varName) {
			data.Remove(varName);
		}

		public string GetString(string varName)
		{
			if (data.ContainsKey(varName))
				return data[varName];
			return ""; // This should actually throw an exception...
		}

		public int GetInt(string varName)
		{
			if (data.ContainsKey(varName))
				return int.Parse(data[varName]);
			return 0; // This should actually throw an exception...
		}

		public bool GetBool(string varName)
		{
			if (data.ContainsKey(varName))
				return bool.Parse(data[varName]);
			return false; // This should actually throw an exception...
		}

		public void CheckUnlocks()
		{
			CheckUnlock("UnlockedWatermelonSeeds");
			CheckUnlock("UnlockedBlueberrySeeds");
			CheckUnlock("UnlockedEggplantSeeds");
			CheckUnlock("UnlockedGrapeSeeds");
			CheckUnlock("UnlockedPumpkinSeeds");
		}

		private void CheckUnlock(string varName)
		{
			int unlockProgress = GetInt(varName);
			if (unlockProgress == 1)
				Set(varName, 2);
		}
	}
}
