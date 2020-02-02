using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class SpeakerNameBox : MonoBehaviour
	{
		public Vector2 boxPosLeft;
		public Vector2 boxPosRight;

		public enum Location { Left, Right }

		private RectTransform rect;
		private Text text;

		public string Text {
			get { return text.text; }
			set {
				text.text = value;
				CheckShow();
			}
		}

		public bool Show {
			get { return show; }
			set {
				show = value;
				CheckShow();
			}
		}
		private bool show;

		public Location BoxLocation {
			get { return location; }
			set {
				if (value == Location.Left)
					rect.anchoredPosition = boxPosLeft;
				else if (value == Location.Right)
					rect.anchoredPosition = boxPosRight;

				location = value;
			}
		}
		private Location location;

		private void Start()
		{
			rect = GetComponent<RectTransform>();
			text = GetComponentInChildren<Text>();
		}

		private void CheckShow() {
			gameObject.SetActive(show && Text.Length > 0);
		}

	}
}