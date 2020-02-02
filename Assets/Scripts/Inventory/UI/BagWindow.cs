using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HorseMoon.Inventory.ItemTypes;
using HorseMoon.Tools;

namespace HorseMoon.Inventory.UI
{
	public class BagWindow : SingletonMonoBehaviour<BagWindow>
	{
		private const float EXTRA_DISPLAY_DURATION = 5f;

		public GameObject ItemUIPrefab;
		private Vector2 itemUISize;

		public Canvas canvas;
		public RectTransform contentHolder;
		public RectTransform cursor;
		public Transform itemHolder;
		public GameObject extraInfoObject;
		public Text itemNameDisplay;
		public Text itemDescDisplay;

		/// <summary>The bag to represent.</summary>
		public Bag Bag
		{
			get { return bag; }
			set {
				// Unsubscribe from the previous bag. -->
				if (bag != null)
				{
					bag.ItemAdded -= OnItemChanged;
					bag.ItemChanged -= OnItemChanged;
				}
				
				// We want to know about this new one! -->
				if (value != null)
				{
					value.ItemAdded += OnItemChanged;
					value.ItemChanged += OnItemChanged;
				}
				
				bag = value;
				ApplyBag();
			}
		}
		private Bag bag;

		private ItemUI[] itemUIs;

		/*public int CursorIndex
		{
			get { return cursorIndex; }
			set {
				if (itemUIs == null || itemUIs.Length == 0)
					return;

				cursorIndex = (int)Mathf.Repeat(value, itemUIs.Length);

				// Place the cursor. -->
				cursor.anchoredPosition = new Vector2(cursorIndex * itemUIWidth, 0f);

				// Change the text to represent newly selected item. -->
				itemNameDisplay.text = $"<b>{HighlightedItemUI.Item.info.displayName}</b>";
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
		}*/

		public bool Visible {
			get { return gameObject.activeSelf;  }
			set {
				gameObject.SetActive(value);
			}
		}

		public int SelectedItemIndex {
			get { return selectedItemIndex; }
			set {
				selectedItemIndex = Mathf.Min(value, bag.Count - 1);

				if (selectedItemIndex == -1)
					return;

				Item item = bag.Get(selectedItemIndex);

				// Select the coorisponding ItemUI. -->
				foreach (ItemUI ui in itemUIs)
				{
					if (ui.Item == item)
					{
						SelectedItemUI = ui;
						break;
					}
				}

				// Tell Player -->
				pc.SetToolForItem(SelectedItem);
			}
		}
		private int selectedItemIndex;

		public Item SelectedItem {
			get {
				if (bag != null)
					return bag.Get(selectedItemIndex);
				return null;
			}
		}

		public ItemUI SelectedItemUI {
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

		public bool ShowExtra
		{
			get { return showExtra; }
			private set {
				bool canShow = value && SelectedItem != null;

				if (canShow)
				{
					// Change the extra info text to represent newly selected item. -->
					itemNameDisplay.text = $"<b>{SelectedItem.info.displayName}</b>";
					itemDescDisplay.text = SelectedItem.info.description;
				}

				extraInfoObject.SetActive(canShow);
				showExtra = canShow;
			}
		}
		private bool showExtra;

		public bool TriggerExtra;

		private float hideExtraTimer;

		private PlayerController pc;

		private void Start()
		{
			cursor.gameObject.SetActive(false);
			itemUIs = new ItemUI[0];
			itemUISize = ItemUIPrefab.GetComponent<RectTransform>().sizeDelta;
			pc = FindObjectOfType<PlayerController>();
			Bag = pc.GetComponent<Bag>();

			ShowExtra = false;
		}

		private void Update()
		{
			HideExtraRoutine();
			CycleItemInputRoutine();

			if (TriggerExtra)
			{
				TriggerExtra = false;
				ShowExtraForDuration(EXTRA_DISPLAY_DURATION);
			}
		}

		private void CycleItemInputRoutine()
		{
			if (Input.GetButtonDown("Previous Item"))
			{
				if (SelectedItemIndex == 0)
					SelectedItemIndex = bag.Count - 1;
				else
					SelectedItemIndex--;

				TriggerExtra = true;
			}
			else if (Input.GetButtonDown("Next Item"))
			{
				if (SelectedItemIndex == bag.Count - 1)
					SelectedItemIndex = 0;
				else
					SelectedItemIndex++;

				TriggerExtra = true;
			}
		}

		private void HideExtraRoutine()
		{
			if (hideExtraTimer > 0f)
			{
				Vector2 leftStick = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);
				hideExtraTimer -= Time.deltaTime + (leftStick.magnitude * 3f * Time.deltaTime);

				if (hideExtraTimer <= 0f || Input.GetButtonDown("Cancel") || Input.GetButtonDown("Use"))
					ShowExtra = false;
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
				rectTransform.anchoredPosition = new Vector2((i % 10) * itemUISize.x, (i / 10) * -itemUISize.y);

				totalWidth += rectTransform.sizeDelta.x;

				// Set the item for it to display. -->
				ItemUI itemUI = ui.GetComponent<ItemUI>();
				itemUI.Item = bag.Get(i);

				itemUIs[i] = itemUI;
			}
			
			contentHolder.sizeDelta = new Vector2(totalWidth, contentHolder.sizeDelta.y);
			SelectedItemIndex = Mathf.Max(selectedItemIndex, 0);
		}

		/// <summary>Remove all items from the panel.</summary>
		private void Clear()
		{
			foreach (ItemUI i in itemUIs)
				Destroy(i.gameObject);
		}

		private void OnItemChanged (Item item)
		{
			ApplyBag();

			if (item.Quantity <= 0)
				TriggerExtra = true;
		}

		private void ShowExtraForDuration(float duration)
		{
			hideExtraTimer = duration;
			ShowExtra = duration > 0f;
		}

	}

}