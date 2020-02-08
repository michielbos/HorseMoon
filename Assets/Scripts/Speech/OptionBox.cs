using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class OptionBox : MonoBehaviour
	{
		private const float STICK_DEAD_ZONE = 0.7f;

		public Animator rouletteAnimator;
		public Text[] rouletteText;
		public Image upPointer;
		public Image downPointer;

		public int OptionIndex {
			get { return optionIndex; }
			set {
				optionIndex = Mathf.Min(value, options.Length - 1);
				UpdateText();

				upPointer.enabled = optionIndex > 0;
				downPointer.enabled = optionIndex < options.Length - 1;
			}
		}
		private int optionIndex;

		private int cancelIndex;

		private string[] options;

		public int SelectedIndex { get; private set; }

		private Vector2 previousLeftStick = Vector2.zero;

		private void Update()
		{
			Vector2 leftStick = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);

			if (ControlStickUp(previousLeftStick, leftStick))
				Previous();
			else if (ControlStickDown(previousLeftStick, leftStick))
				Next();
			else if (Input.GetButtonDown("Use") || Input.GetButtonDown("Pause"))
				SelectedIndex = optionIndex;
			else if (Input.GetKey(KeyCode.BackQuote) && Input.GetButton("Cancel"))
				SelectedIndex = cancelIndex > -1 ? cancelIndex : 0;
			else if (Input.GetButtonDown("Cancel") && cancelIndex > -1)
				SelectedIndex = cancelIndex;

			previousLeftStick = leftStick;
		}

		public void Show (string[] newOptions)
		{
			gameObject.SetActive(true);
			SelectedIndex = -1;
			options = newOptions;
			OptionIndex = 0;
			cancelIndex = -1;

			// Is there a "cancel" option? -->
			for (int i = 0; i < newOptions.Length; i++)
			{
				if (newOptions[i].Length > 0 && newOptions[i][0] == '!')
				{
					cancelIndex = i;
					options[i] = newOptions[i].Substring(1);
					break;
				}
			}
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Previous ()
		{
			if (OptionIndex > 0)
			{
				OptionIndex--;

				rouletteAnimator.Play("Up", 0, 0f);
			}
		}

		public void Next()
		{
			if (OptionIndex < options.Length - 1)
			{
				OptionIndex++;
				rouletteAnimator.Play("Down", 0, 0f);
			}
		}

		private void UpdateText()
		{
			rouletteText[0].text = GetOptionText(optionIndex - 2);
			rouletteText[1].text = GetOptionText(optionIndex - 1);
			rouletteText[2].text = GetOptionText(optionIndex);
			rouletteText[3].text = GetOptionText(optionIndex + 1);
			rouletteText[4].text = GetOptionText(optionIndex + 2);
		}

		private string GetOptionText(int index)
		{
			if (index >= 0 && index < options.Length)
				return options[index];
			return "";
		}

		private bool ControlStickUp(Vector2 previous, Vector2 current) {
			return previous.y < STICK_DEAD_ZONE && current.y >= STICK_DEAD_ZONE;
		}

		private bool ControlStickDown(Vector2 previous, Vector2 current) {
			return previous.y > -STICK_DEAD_ZONE && current.y <= -STICK_DEAD_ZONE;
		}
	}
}