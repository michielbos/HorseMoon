using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HorseMoon.Inventory.UI
{
	public class BagCursor : MonoBehaviour
	{
		public BagWindow window;

		private GameObject lastSelectedItemUI;

		private void Update()
		{
			if (lastSelectedItemUI != EventSystem.current.currentSelectedGameObject)
				Highlight(EventSystem.current.currentSelectedGameObject);
		}

		private void Highlight(GameObject nextItemUI)
		{
			lastSelectedItemUI = EventSystem.current.currentSelectedGameObject;
		}
	}
}