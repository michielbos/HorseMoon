using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Inventory.ItemTypes
{
	public class ToolInfo : ItemInfo
	{
		public enum Type { None, Hoe, WateringCan, Sickle, Hammer }

		public Type type;
	}
}