using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory
{
	/// <summary>
	/// An Item for a Bag. Items...
	///		-Have a name and picture
	///		-Have a quantity
	///		-Can be used
	/// </summary>
	public class Item
	{
		public int quantity;

		public readonly ItemInfo info;

		public Item(ItemInfo info, int quantity)
		{
			this.info = info;
			this.quantity = quantity;
		}

		public Item(string infoName, int quantity) : this(ItemInfo.Get(infoName), quantity) { }

		public override string ToString()
		{
			return $"{info.displayName} x{quantity}";
		}
	}
}