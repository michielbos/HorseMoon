using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon
{
	public class StoryProgress : SingletonMonoBehaviour<StoryProgress>
	{
		private Dictionary<string, string> data = new Dictionary<string, string>();

		private void Start()
		{
			Set("TTNextChat", "TenderTill.Chat1");
		}

		public void Set(string varName, string value) {
			data[varName] = value;
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
			return -1; // This should actually throw an exception...
		}

		public bool GetBool(string varName)
		{
			if (data.ContainsKey(varName))
				return bool.Parse(data[varName]);
			return false; // This should actually throw an exception...
		}
	}
}
