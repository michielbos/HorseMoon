using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory
{
	/// <summary>
	/// A Bag contains Item objects. You can...<br></br>
	///		-Add an Item object. Similar items will be combined together.
	/// </summary>
	public class Bag : MonoBehaviour
	{
		private List<Item> items;

		public int Size => items.Capacity;

		public int Count => items.Count;

		public int MaxWeight;

		public int TotalWeight {
			get {
				int weight = 0;
				foreach (Item i in items)
					weight += i.Weight;
				return weight;
			}
		}

		public event Item.ItemEvent ItemChanged;

		private void Start()
		{
			items = new List<Item>();
			Add("NoTool", 0);
			Add("Hoe", 1);
			Add("WateringCan", 1);
			Add("Sickle", 1);
			
			// Example Items -->
			Add("StrawberrySeeds", 5);
			Add("CarrotSeeds", 5);
			Add("CornSeeds", 5);
		}

		private void OnDestroy()
		{
			foreach (Item i in items)
				i.QuantityChange -= OnItemQuantityChange;
		}

		/// <summary>
		/// Add an Item to the Bag. If the Bag already has an Item of the same kind, their quantity will be combined.<br></br>
		/// It returns 'false' if the item can't be added. (Too much weight)
		/// </summary>
		public bool Add(Item newItem)
		{
			// Can't add this if the bag is too filled. -->
			if (TotalWeight + newItem.Weight > MaxWeight)
				return false;

			Item i = Get(newItem.info);

			if (i != null)
			{
				// Combine these two together. -->
				i.Quantity += newItem.Quantity;
			}
			else
			{
				// It's a new item. -->
				newItem.QuantityChange += OnItemQuantityChange;
				items.Add(newItem);
			}

			return true;
		}

		public bool CanAdd(ItemInfo info, int amount) {
			return TotalWeight + (info.weight * amount) <= MaxWeight;
		}

		public bool Add(ItemInfo info, int amount)
		{
			// Can't add this if the bag is too filled. -->
			if (!CanAdd(info, amount))
				return false;

			Item i = Get(info);

			if (i != null)
			{
				// Combine these two together. -->
				i.Quantity += amount;
			}
			else
			{
				// It's a new item. -->
				Item newItem = new Item(info, amount);
				newItem.QuantityChange += OnItemQuantityChange;
				items.Add(newItem);
			}

			return true;
		}

		public bool Add(string infoName, int amount) {
			return Add(ItemInfo.Get(infoName), amount);
		}

		public bool CanUse(Item item) {
			return items.Contains(item) && item.Quantity > 0;
		}

		public Item Get(ItemInfo info)
		{
			// Do we have this item? -->
			foreach (Item i in items)
				if (i.info == info)
					return i;
			return null;
		}

		public Item Get(string infoName) {
			return Get(ItemInfo.Get(infoName));
		}

		public Item Get(int index)
		{
			if (index < items.Count)
				return items[index];
			return null;
		}

		public bool Contains (Item item) {
			return items.Contains(item);
		}

		public bool Remove(Item item)
		{
			if (items.Contains(item))
			{
				item.QuantityChange -= OnItemQuantityChange;
				items.Remove(item);
				return true;
			}

			return false;
		}

		public bool Remove(ItemInfo kind)
		{
			Item i = Get(kind);

			if (i == null)
				return false;

			i.QuantityChange -= OnItemQuantityChange;
			items.Remove(i);
			return true;
		}

		private void OnItemQuantityChange(Item item)
		{
			// Remove the item if it was all used up. -->
			if (item.Quantity <= 0)
				Remove(item);

			ItemChanged?.Invoke(item);
		}

		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}

		public override string ToString()
		{
			string s = $"Size: {Size}, Count: {Count}";

			foreach (Item i in items)
				s += $", {{{i}}}";

			return s;
		}
	}
}