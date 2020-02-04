using System;
using UnityEngine;

namespace HorseMoon.Speech {

	/// <summary>This seems silly...</summary>
	public class SpeechCharacterData : ScriptableObject
	{
		public string[] names;
		public RuntimeAnimatorController animatorController;

		public static SpeechCharacterData Load(string dataName) {
			return Resources.Load<SpeechCharacterData>($"Speech/Characters/{dataName}"); ;
		}
	}
}