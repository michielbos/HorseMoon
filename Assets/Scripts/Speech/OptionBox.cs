using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class OptionBox : MonoBehaviour
	{
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

		private string[] options;

		public int selectedIndex { get; private set; }

		private void Start()
		{
			
		}

		private void Update()
		{
			if (Input.GetButtonDown("Previous Item"))
				Previous();
			else if (Input.GetButtonDown("Next Item"))
				Next();
			else if (Input.GetButtonDown("Use"))
				selectedIndex = optionIndex;
		}

		public void ShowOptions (string[] newOptions)
		{
			gameObject.SetActive(true);
			selectedIndex = -1;
			options = newOptions;
			OptionIndex = 0;
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
	}
}