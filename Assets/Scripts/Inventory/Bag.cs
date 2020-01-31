using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory
{
	/// <summary>
	/// A Bag contains Item objects. You can...<br></br>
	///		-Add an Item object. Similar items will be combined together.
	/// </summary>
	public class Bag
	{
		private List<Item> items;

		public int Size => items.Capacity;

		public int Count => items.Count;

		/// <summary>
		/// Make an empty bag that can hold X different kinds of items.
		/// </summary>
		public Bag(int size)
		{
			items = new List<Item>(size);
		}

		/// <summary>
		/// Add an Item to the Bag. If the Bag already has an Item of the same kind, their quantity will be combined.
		/// </summary>
		public void Add(Item newItem)
		{
			Item i = Get(newItem.info);

			if (i != null)
			{
				// Combine these two together. -->
				i.quantity += newItem.quantity;
			}
			else
			{
				// It's a new item. -->
				items.Add(newItem);
			}
		}

		public void Add(ItemInfo info, int amount)
		{
			Item i = Get(info);

			if (i != null)
			{
				// Combine these two together. -->
				i.quantity += amount;
			}
			else
			{
				// It's a new item. -->
				items.Add(new Item(info, amount));
			}
		}

		public void Add(string infoName, int amount)
		{
			Add(ItemInfo.Get(infoName), amount);
		}

		/*public bool Use(Item item)
		{
			// This should be an item from this bag... -->
			if (!items.Contains(item))
				return false;

			// Use the item. -->
			item.quantity--;

			// Remove the item if that was the last one. -->
			if (item.quantity <= 0)
				items.Remove(item);

			return true;
		}*/

		public Item Get(ItemInfo info)
		{
			// Do we have this item? -->
			foreach (Item i in items)
				if (i.info == info)
					return i;
			return null;
		}

		public Item Get(string infoName)
		{
			return Get(ItemInfo.Get(infoName));
		}

		public Item Get(int index)
		{
			if (index < items.Count)
				return items[index];
			return null;
		}

		public bool Remove(Item item)
		{
			if (items.Contains(item))
			{
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

			items.Remove(i);
			return true;
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