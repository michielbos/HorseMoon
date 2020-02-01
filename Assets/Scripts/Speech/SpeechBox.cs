﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class SpeechBox : MonoBehaviour
	{
		private Image box;
		private Text text;

		public Color dimColor;

		public string Text
		{
			get { return text.text; }
			set
			{
				text.text = value;
			}
		}

		public bool UseDim
		{
			get { return useDim; }
			set
			{
				box.color = value ? dimColor : Color.white;
				text.color = value ? dimColor : Color.white;
				useDim = value;
			}
		}
		private bool useDim;

		public bool UsePopupFormat
		{
			get { return usePopupFormat; }
			set {
				text.alignment = value ? TextAnchor.MiddleCenter : TextAnchor.UpperLeft;
				usePopupFormat = value;
			}
		}
		private bool usePopupFormat;

		private void Start()
		{
			box = GetComponent<Image>();
			text = GetComponentInChildren<Text>();
		}

	}
}
