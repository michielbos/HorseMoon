using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorseMoon.Speech
{
	public class SpeechTrigger : MonoBehaviour
	{
		public string node;
		public string requiredStoryVar;
		public int requiredStoryValue;

		private void OnTriggerEnter2D(Collider2D c)
		{
			Player p = c.GetComponent<Player>();
			if (p != null)
				TryTrigger();
		}

		private void TryTrigger()
		{
			if (StoryProgress.Instance.GetInt(requiredStoryVar) == requiredStoryValue)
				SpeechUI.Instance.Behavior.StartDialogue(node);
		}
	}
}
