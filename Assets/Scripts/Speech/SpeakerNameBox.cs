using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class SpeakerNameBox : MonoBehaviour
	{
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

		private void Start()
		{
			text = GetComponentInChildren<Text>();
		}

		private void CheckShow() {
			gameObject.SetActive(show && Text.Length > 0);
		}

	}
}