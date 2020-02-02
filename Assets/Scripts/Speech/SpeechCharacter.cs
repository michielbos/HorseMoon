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

		public bool IsVisible => image.sprite != null;

		private void Start()
		{
			image = GetComponent<Image>();
		}

		private SpeechCharacterData.Expression GetExpression(string exprName)
		{
			if (data != null)
			{
				SpeechCharacterData.Expression[] exprSet = Speaking ? data.speakExpressions : data.expressions;

				foreach (SpeechCharacterData.Expression e in exprSet)
					if (e.name.Equals(exprName))
						return e;
			}

			return new SpeechCharacterData.Expression();
		}

		private void UpdateExpression()
		{
			SpeechCharacterData.Expression expr = GetExpression(expression);
			image.sprite = data != null ? expr.sprite : null;
			image.enabled = image.sprite != null;
		}

	}
}