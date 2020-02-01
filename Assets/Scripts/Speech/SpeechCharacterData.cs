using System;
using UnityEngine;

namespace HorseMoon.Speech {

	/// <summary>This seems silly...</summary>
	public class SpeechCharacterData : ScriptableObject
	{
		[Serializable]
		public struct Expression {
			public string name;
			public Sprite sprite;
		}

		public string[] names;

		public Expression[] expressions;
	}
}