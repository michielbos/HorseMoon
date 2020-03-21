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

		public Coroutine FadeOut() {
			return Fade(1f, Color.clear, Color.black, fadedOut);
		}

		public Coroutine FadeOut(float duration) {
			return Fade(duration, Color.clear, Color.black, fadedOut);
		}

		public Coroutine FadeOut(float duration, Color fromColor, Color toColor) {
			return Fade(duration, fromColor, toColor, fadedOut);
		}

		public Coroutine FadeIn() {
			return Fade(1f, Color.black, Color.clear, fadedIn);
		}

		public Coroutine FadeIn(float duration) {
			return Fade(duration, Color.black, Color.clear, fadedIn);
		}

		public Coroutine FadeIn(float duration, Color fromColor, Color toColor) {
			return Fade(duration, fromColor, toColor, fadedIn);
		}

		private Coroutine Fade(float duration, Color fromColor, Color toColor, System.Action eventDele)
		{
			if (doFadeCoroutine != null)
				StopCoroutine(doFadeCoroutine);
			doFadeCoroutine = StartCoroutine(DoFade(duration, fromColor, toColor, eventDele));
			return doFadeCoroutine;
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