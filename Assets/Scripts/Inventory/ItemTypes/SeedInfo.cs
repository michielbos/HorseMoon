using System.Collections;
using System.Collections.Generic;

namespace HorseMoon.Inventory.ItemTypes
{
	// There should be a list somewhere of the kinds of plants that will exist...
	// public enum PlantType { Sample, Yay }

	public class SeedInfo : ItemInfo
	{
		// public PlantType plantType;
		public CropData plantType;
	}
}