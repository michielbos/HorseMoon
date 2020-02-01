using System.Collections;
using System.Collections.Generic;

namespace HorseMoon.Inventory.ItemTypes
{
	public class FoodInfo : ItemInfo
	{
		/// <summary>The amount of energy this kind of food item will restore when eaten.</summary>
		public int energy;

		public int shipValue;
	}
}