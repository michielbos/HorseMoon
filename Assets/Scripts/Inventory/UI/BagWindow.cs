using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HorseMoon.Inventory.ItemTypes;
using HorseMoon.Tools;

namespace HorseMoon.Inventory.UI
{
	public class BagWindow : MonoBehaviour
	{
		private const float EXTRA_DISPLAY_DURATION = 5f;

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
				// Unsubscribe from the previous bag. -->
				if (bag != null)
					bag.ItemChanged -= OnItemChanged;

				// We want to know about this new one! -->
				if (value != null)
					value.ItemChanged += OnItemChanged;

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

		public int SelectedItemIndex {
			get { return selectedItemIndex; }
			set {
				selectedItemIndex = Mathf.Min(value, bag.Count - 1);

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

				selectedItemIndex = value;
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
			set {
				if (value)
				{
					// Change the extra info text to represent newly selected item. -->
					itemNameDisplay.text = $"<b>{SelectedItem.info.displayName}</b>";
					itemDescDisplay.text = SelectedItem.info.description;
				}

				extraInfoObject.SetActive(value);
				showExtra = value;
			}
		}
		private bool showExtra;

		private float hideExtraTimer;

		private PlayerController pc;

		private void Start()
		{
			cursor.gameObject.SetActive(false);
			itemUIs = new ItemUI[0];
			itemUIWidth = ItemUIPrefab.GetComponent<RectTransform>().sizeDelta.x;
			pc = FindObjectOfType<PlayerController>();
			Bag = pc.GetComponent<Bag>();

			ShowExtra = false;
		}

		private void Update()
		{
			HideExtraRoutine();
			CycleItemInputRoutine();
		}

		private void CycleItemInputRoutine()
		{
			if (Input.GetButtonDown("Previous Item"))
			{
				if (SelectedItemIndex == 0)
					SelectedItemIndex = bag.Count - 1;
				else
					SelectedItemIndex--;

				ShowExtraForDuration(EXTRA_DISPLAY_DURATION);
			}
			else if (Input.GetButtonDown("Next Item"))
			{
				if (SelectedItemIndex == bag.Count - 1)
					SelectedItemIndex = 0;
				else
					SelectedItemIndex++;

				ShowExtraForDuration(EXTRA_DISPLAY_DURATION);
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
				rectTransform.anchoredPosition = new Vector2(itemUIWidth * i, 0f);

				totalWidth += rectTransform.sizeDelta.x;

				// Set the item for it to display. -->
				ItemUI itemUI = ui.GetComponent<ItemUI>();
				itemUI.Item = bag.Get(i);

				itemUIs[i] = itemUI;
			}
			
			contentHolder.sizeDelta = new Vector2(totalWidth, contentHolder.sizeDelta.y);
			SelectedItemIndex = selectedItemIndex;
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
		}

		public void ShowExtraForDuration(float duration)
		{
			hideExtraTimer = duration;
			ShowExtra = duration > 0f;
		}

	}

}