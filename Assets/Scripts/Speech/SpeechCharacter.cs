using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.Speech
{
	public class SpeechCharacter : MonoBehaviour
	{
		private Image image;

		public SpeechCharacterData data { get; private set; }

		public string DataName {
			get {
				if (data != null)
					return data.name;
				return "";
			}
			set {
				data = Resources.Load<SpeechCharacterData>($"Speech/Characters/{value}"); // Temporary.
				Expression = "Normal";
			}
		}

		public string Expression {
			get { return expression.name; }	
			set {
				expression = GetExpression(value);
				image.sprite = data != null ? expression.sprite : null;
				image.enabled = image.sprite != null;
			}
		}
		private SpeechCharacterData.Expression expression;

		public bool IsVisible => image.sprite != null;

		private void Start()
		{
			image = GetComponent<Image>();
		}

		private SpeechCharacterData.Expression GetExpression(string exprName)
		{
			if (data != null)
			{
				foreach (SpeechCharacterData.Expression e in data.expressions)
					if (e.name.Equals(exprName))
						return e;
			}

			return new SpeechCharacterData.Expression();
		}

	}
}