using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory.ItemTypes
{
    // There should be a list somewhere of the kinds of plants that will exist...
    // public enum PlantType { Sample, Yay }
    [CreateAssetMenu(fileName = "SeedInfo", menuName = "HorseMoon/Seed Info")]
    public class SeedInfo : ItemInfo
	{
		// public PlantType plantType;
		public CropData plantType;
	}
}