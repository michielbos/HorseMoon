using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HorseMoon.Inventory.ItemTypes;

namespace HorseMoon.Inventory.UI
{
	public class ItemUI : MonoBehaviour
	{
		public Image sprite;
		public Image selectedBG;
		public Text quantityDisplay;

		/// <summary>The item this UI represents.</summary>
		public Item Item {
			get { return item; }
			set {
				if (value != null)
				{
					sprite.enabled = value.info.sprite != null;
					sprite.sprite = value.info.sprite;
					quantityDisplay.enabled = value.info.GetType() != typeof(ToolInfo);
					quantityDisplay.text = $"x{value.Quantity}";
				}

				item = value;
			}
		}
		private Item item;

		public bool Selected {
			get { return selected; }
			set {
				selectedBG.enabled = value && item != null && item.info.sprite != null;
				selected = value;
			}
		}
		private bool selected;
	}
}