using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon
{
	public static class StoryProgress
	{
		private static Dictionary<string, string> data = new Dictionary<string, string>();

		public static void Set(string varName, string value)
		{
			if (data.ContainsKey(varName))
				data.Add(varName, value);
			else
				data[varName] = value;
		}

		public static void Clear(string varName) {
			data.Remove(varName);
		}

		public static string GetString(string varName)
		{
			if (data.ContainsKey(varName))
				return data[varName];
			return ""; // This should actually throw an exception...
		}

		public static int GetInt(string varName)
		{
			if (data.ContainsKey(varName))
				return int.Parse(data[varName]);
			return -1; // This should actually throw an exception...
		}

		public static bool GetBool(string varName)
		{
			if (data.ContainsKey(varName))
				return bool.Parse(data[varName]);
			return false; // This should actually throw an exception...
		}
	}
}
