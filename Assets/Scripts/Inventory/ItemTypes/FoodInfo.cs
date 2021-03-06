using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory.ItemTypes
{
    [CreateAssetMenu(fileName = "NewFoodInfo", menuName = "HorseMoon/Food Info")]
    public class FoodInfo : ItemInfo
	{
		/// <summary>The amount of energy this kind of food item will restore when eaten.</summary>
		public int energy;

		public int shipValue;
	}
}