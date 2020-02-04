using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class SpeechCharacter : MonoBehaviour
	{
		private Animator animator;
		private Image image;

		public SpeechCharacterData Data { get; private set; }

		public string DataName {
			get {
				if (Data != null)
					return Data.name;
				return "";
			}
			set {
				Data = SpeechCharacterData.Load(value);
				if (Data != null)
					animator.runtimeAnimatorController = Data.animatorController;
				Expression = "";
			}
		}

		public string Expression {
			get { return expression; }	
			set {
				expression = value;
				UpdateExpression();
			}
		}
		private string expression;

		public bool Speaking {
			get { return speaking; }
			set {
				speaking = value;
				UpdateExpression();
			}
		}
		private bool speaking;

		public bool IsVisible => image.enabled;

		private void Start()
		{
			animator = GetComponent<Animator>();
			image = GetComponentInChildren<Image>();
		}

		private void UpdateExpression()
		{
			if (Data == null || Data.animatorController == null)
			{
				image.enabled = false;
				return;
			}

			string animation = expression;

			if (Speaking)
				animation += "Talk";

			if (animator.HasState(0, Animator.StringToHash(animation)))
			{
				image.enabled = true;
				animator.Play(animation);
			}
			else
				image.enabled = false;
		}

	}
}