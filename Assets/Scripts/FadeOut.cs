using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon
{
	public class FadeOut : SingletonMonoBehaviour<FadeOut>
	{
		private const float FADE_OUT_DURATION = 2f;
		private const float FADE_IN_DURATION = 1f;

		private Canvas canvas;
		private RawImage blackImage;

		private Coroutine doFadeOutCoroutine;

		public System.Action fadedOut;
		public System.Action fadedIn;

		private new void Awake()
		{
			base.Awake();
			canvas = GetComponentInChildren<Canvas>();
			blackImage = GetComponentInChildren<RawImage>();
		}

		public void Begin()
		{
			if (doFadeOutCoroutine != null)
				StopCoroutine(doFadeOutCoroutine);
			doFadeOutCoroutine = StartCoroutine(DoFadeOut());
		}

		private IEnumerator DoFadeOut()
		{
			float t = 0f;

			// Fade Out -->
			while (t < 1f)
			{
				t += Time.deltaTime / FADE_OUT_DURATION;
				blackImage.color = Color.Lerp(Color.clear, Color.black, t);
				yield return null;
			}

			fadedOut?.Invoke();

			// Fade In -->
			t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime / FADE_IN_DURATION;
				blackImage.color = Color.Lerp(Color.black, Color.clear, t);
				yield return null;
			}

			fadedIn?.Invoke();
			doFadeOutCoroutine = null;
		}
	}
}
