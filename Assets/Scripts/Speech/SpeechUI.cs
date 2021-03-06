﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Speech
{
	/// <summary>What a wrap.</summary>
	public class SpeechUI : SingletonMonoBehaviour<SpeechUI>
	{
		public SpeechUIBehavior Behavior { get; private set; }

		private new void Awake()
		{
			base.Awake();
			Behavior = GetComponent<SpeechUIBehavior>();
		}
	}
}
