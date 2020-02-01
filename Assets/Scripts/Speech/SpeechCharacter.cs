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
				image.sprite = expression.sprite;
			}
		}
		private SpeechCharacterData.Expression expression;

		private void Start()
		{
			image = GetComponent<Image>();
		}

		private SpeechCharacterData.Expression GetExpression(string exprName)
		{
			foreach (SpeechCharacterData.Expression e in data.expressions)
				if (e.name.Equals(exprName))
					return e;

			return new SpeechCharacterData.Expression();
		}

	}
}