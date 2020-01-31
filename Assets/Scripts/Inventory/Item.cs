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
		public int Quantity {
			get { return quantity; }
			set {
				quantity = value;
				QuantityChange?.Invoke(this);
			}
		}
		private int quantity;

		public readonly ItemInfo info;

		public delegate void ItemEvent(Item item);
		public event ItemEvent QuantityChange;

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