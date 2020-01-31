using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HorseMoon.Inventory.ItemTypes;

namespace HorseMoon.Inventory.UI
{
	public class BagWindow : MonoBehaviour
	{
		[SerializeField] private GameObject ItemUIPrefab;
		private float itemUIWidth;

		[SerializeField] private RectTransform contentHolder;
		[SerializeField] private RectTransform cursor;
		[SerializeField] private Transform itemHolder;
		[SerializeField] private GameObject extraInfoObject;
		[SerializeField] private Text itemNameDisplay;
		[SerializeField] private Text itemDescDisplay;

		/// <summary>The bag to represent.</summary>
		public Bag Bag
		{
			get { return bag; }
			set {
				bag = value;
				SelectedItemUI = null;
			}
		}
		private Bag bag;

		private ItemUI[] itemUIs;

		public int CursorIndex
		{
			get { return cursorIndex; }
			set {
				if (itemUIs == null || itemUIs.Length == 0)
					return;

				cursorIndex = (int)Mathf.Repeat(value, itemUIs.Length);

				// Place the cursor. -->
				cursor.anchoredPosition = new Vector2(cursorIndex * itemUIWidth, 0f);

				// Change the text to represent newly selected item. -->
				itemNameDisplay.text = HighlightedItemUI.Item.info.displayName;
				itemDescDisplay.text = HighlightedItemUI.Item.info.description;
			}
		}
		private int cursorIndex;

		public ItemUI HighlightedItemUI {
			get {
				if (cursorIndex > -1 && cursorIndex < itemUIs.Length)
					return itemUIs[cursorIndex];
				return null;
			}
			set {
				for (int i = 0; i < itemUIs.Length; i++)
				{
					if (itemUIs[i] == value)
					{
						CursorIndex = i;
						return;
					}
				}
			}
		}

		public ItemUI SelectedItemUI
		{
			get { return selectedItemUI; }
			set {
				// Unselect Previous -->
				if (selectedItemUI != null)
					selectedItemUI.Selected = false;

				// Select this new one. -->
				if (value != null)
					value.Selected = true;

				selectedItemUI = value;
			}
		}
		private ItemUI selectedItemUI;

		public bool Active
		{
			get { return active; }
			set {
				if (value && selectedItemUI != null)
					HighlightedItemUI = selectedItemUI;

				cursor.gameObject.SetActive(value);
				extraInfoObject.SetActive(value);
				active = value;

				if (pc != null)
					pc.enabled = !value;
			}
		}
		private bool active;

		private PlayerController pc;

		private void Start()
		{
			itemUIs = new ItemUI[0];
			itemUIWidth = ItemUIPrefab.GetComponent<RectTransform>().sizeDelta.x;
			pc = FindObjectOfType<PlayerController>();

			Active = false;

			// DEBUG: Make a sample bag. -->
			bag = new Bag(20);
			bag.Add("NoTool", 0);
			bag.Add("Hoe", 1);
			bag.Add("WateringCan", 1);
			bag.Add("YaySeeds", 5);
			bag.Add("ExampleFood", 2);
			ApplyBag();
		}

		private void Update()
		{
			// DEBUG -->
			if (Input.GetKeyDown(KeyCode.L))
			{
				ApplyBag();
			}

			if (!Active)
			{
				// Can't activate if there are no items. -->
				if (Input.GetButtonDown("Inventory"))
				{
					if (itemUIs.Length > 0)
						Active = true;
				}
			}
			else
			{
				if (Input.GetButtonDown("Inventory") || Input.GetButtonDown("Cancel"))
					Active = false;
				else if (Input.GetButtonDown("Previous Item"))
					CursorIndex--;
				else if (Input.GetButtonDown("Next Item"))
					CursorIndex++;
				else if (Input.GetButtonDown("Use"))
				{
					// Select the highlighted item. -->
					SelectedItemUI = HighlightedItemUI;
					ChooseItem();

					Active = false;
				}
			}
		}

		private void ApplyBag()
		{
			Clear();

			if (bag == null)
				return;

			float totalWidth = 0f;

			itemUIs = new ItemUI[bag.Count];

			for (int i = 0; i < bag.Count; i++)
			{
				GameObject ui = Instantiate(ItemUIPrefab, itemHolder, false);

				// Position based on index and size. -->
				RectTransform rectTransform = ui.GetComponent<RectTransform>();
				rectTransform.anchoredPosition = new Vector2(itemUIWidth * i, 0f);

				totalWidth += rectTransform.sizeDelta.x;

				// Set the item for it to display. -->
				ItemUI itemUI = ui.GetComponent<ItemUI>();
				itemUI.Item = bag.Get(i);

				itemUIs[i] = itemUI;
			}
			
			contentHolder.sizeDelta = new Vector2(totalWidth, contentHolder.sizeDelta.y);

			CursorIndex = 0;
		}

		/// <summary>Remove all items from the panel.</summary>
		private void Clear()
		{
			foreach (ItemUI i in itemUIs)
				Destroy(i.gameObject);
		}

		private void ChooseItem()
		{
			// Tell the player object about it! -->
			if (pc != null)
			{
				Item item = SelectedItemUI.Item;
				System.Type itemType = item.info.GetType();

				if (itemType == typeof(ToolInfo))
				{
					ToolInfo tool = (ToolInfo)item.info;
					pc.SelectTool(pc.tools[(int)tool.type]);
				}
				else
					pc.SelectTool(pc.tools[0]);
			}
		}
	}

}