using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.UI
{
	public class UICanvasController : SingletonMonoBehaviour<UICanvasController>
	{
		private Canvas canvas;
		
		public bool Visible {
			get => canvas.enabled;
			set { canvas.enabled = value; }
		}

		private new void Awake()
		{
			base.Awake();
			canvas = GetComponent<Canvas>();
		}
	}
}

