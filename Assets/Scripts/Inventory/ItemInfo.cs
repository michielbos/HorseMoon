using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory
{
	public abstract class ItemInfo : ScriptableObject
	{
		public string displayName;
		public Sprite sprite;
		public string description;
		public int weight;


		private static Dictionary<string, ItemInfo> allInfos;

		/// <summary>
		/// Returns information for the specified kind of item.
		/// </summary>
		public static ItemInfo Get(string infoName)
		{
			if (allInfos == null)
				Init();

			if (allInfos.ContainsKey(infoName))
				return allInfos[infoName];
			
			Debug.LogWarning($"Item {infoName} doesn't exist.");
			return null;
		}

		/// <summary>This is probably a silly way to do this.</summary>
		public static void Init()
		{
			// Load all items. -->
			ItemInfo[] infos = Resources.LoadAll<ItemInfo>("Items");

			// We'll pair them with their names... -->
			allInfos = new Dictionary<string, ItemInfo>();

			foreach (ItemInfo i in infos)
				allInfos.Add(i.name, i);
		}
	}
}