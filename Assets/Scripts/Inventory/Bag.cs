using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HorseMoon.Inventory.Item;

namespace HorseMoon.Inventory
{
	/// <summary>
	/// A Bag contains Item objects. You can...<br></br>
	///		-Add an Item object. Similar items will be combined together.
	/// </summary>
	public class Bag : MonoBehaviour
	{
		private List<Item> items = new List<Item>();

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

		public event ItemEvent ItemAdded;
		public event ItemEvent ItemChanged;

		private void Start()
		{
			Add("NoTool", 0);
			Add("Hoe", 1);
			Add("WateringCan", 1);
			Add("Sickle", 1);
			Add("Hammer", 1);
			Add("Axe", 1);
			
			// Example Items -->
			Add("StrawberrySeeds", 5);
			Add("CarrotSeeds", 5);
			Add("CornSeeds", 5);
			
			// Food for testing
			Add("Strawberries", 3);
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
			if (!CanAdd(newItem))
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

				ItemAdded?.Invoke(newItem);
			}

			return true;
		}

		public bool CanAdd(Item i) {
			return TotalWeight + (i.info.weight * i.Quantity) <= MaxWeight;
		}

		public bool CanAdd(ItemInfo info, int amount) {
			return TotalWeight + (info.weight * amount) <= MaxWeight;
		}

		public bool CanAdd(string infoName, int amount) {
			return CanAdd(ItemInfo.Get(infoName), amount);
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

				ItemAdded?.Invoke(newItem);
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
			if (index >= 0 && index < items.Count)
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
				ItemChanged?.Invoke(item);
				return true;
			}

			return false;
		}

		public bool Remove(ItemInfo info, int amount)
		{
			Item i = Get(info);

			if (i == null || i.Quantity < amount)
				return false;

			i.Quantity -= amount;

			if (i.Quantity <= 0)
				items.Remove(i);

			return true;
		}

		public bool Remove(string infoName, int amount) {
			return Remove(ItemInfo.Get(infoName), amount);
		}

		private void OnItemQuantityChange(Item item)
		{
			// Remove the item if it was all used up. -->
			if (item.Quantity <= 0)
				Remove(item);
			else
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

		public ItemData[] GetItemDatas() {
			List<ItemData> itemDatas = new List<ItemData>(items.Count);
			foreach (Item item in items) {
				// The NoTool is never deleted, so don't save it either.
				if (item.info.name == "NoTool")
					continue;
				itemDatas.Add(item.GetData());
			}
			return itemDatas.ToArray();
		}

		public void SetItemsDatas(ItemData[] itemDatas) {
			Clear();
			foreach (ItemData itemData in itemDatas) {
				Debug.Log(itemData.key);
				Add(itemData.key, itemData.quantity);
			}
		}

		public void Clear() {
			Item[] itemsCopy = items.ToArray();
			foreach (Item item in itemsCopy) {
				// Never delete the NoTool.
				if (item.info.name == "NoTool")
					continue;
				Remove(item);
			}
		}
	}
}