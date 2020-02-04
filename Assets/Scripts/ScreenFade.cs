using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon
{
	public class ScreenFade : SingletonMonoBehaviour<ScreenFade>
	{
		//private Canvas canvas;
		private RawImage blackImage;

		private Coroutine doFadeCoroutine;

		public System.Action fadedOut;
		public System.Action fadedIn;

		private new void Awake()
		{
			base.Awake();
			//canvas = GetComponentInChildren<Canvas>();
			blackImage = GetComponentInChildren<RawImage>();
		}

		public void FadeOut() {
			Fade(1f, Color.clear, Color.black, fadedOut);
		}

		public void FadeOut(float duration) {
			Fade(duration, Color.clear, Color.black, fadedOut);
		}

		public void FadeOut(float duration, Color fromColor, Color toColor) {
			Fade(duration, fromColor, toColor, fadedOut);
		}

		public void FadeIn() {
			Fade(1f, Color.black, Color.clear, fadedIn);
		}

		public void FadeIn(float duration) {
			Fade(duration, Color.black, Color.clear, fadedIn);
		}

		public void FadeIn(float duration, Color fromColor, Color toColor) {
			Fade(duration, fromColor, fromColor, fadedIn);
		}

		private void Fade(float duration, Color fromColor, Color toColor, System.Action eventDele)
		{
			if (doFadeCoroutine != null)
				StopCoroutine(doFadeCoroutine);
			doFadeCoroutine = StartCoroutine(DoFade(duration, fromColor, toColor, eventDele));
		}

		private IEnumerator DoFade(float duration, Color fromColor, Color toColor, System.Action eventDele)
		{
			float t = 0f;

			while (t < 1f)
			{
				t += Time.unscaledDeltaTime / duration;
				blackImage.color = Color.Lerp(fromColor, toColor, t);
				yield return null;
			}

			eventDele?.Invoke();
			doFadeCoroutine = null;
		}
	}
}