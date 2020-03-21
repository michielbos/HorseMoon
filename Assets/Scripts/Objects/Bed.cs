using System.Collections;
using UnityEngine;
using HorseMoon.Speech;

namespace HorseMoon.Objects {

    public class Bed : InteractionObject
    {
        public AudioClip sleepMusic;

        public override bool CanUse(Player player) {
            return true;
        }

        public override void UseObject(Player player) {
            SpeechUI.Instance.Behavior.StartDialogue("Bed");
        }

        public void Sleep() {
            StartCoroutine(SleepRoutine());
        }

        private IEnumerator SleepRoutine()
        {
            // Fade Out -->
            FindObjectOfType<MusicPlayer>().PlaySong(sleepMusic);
            GameplayManager.Instance.AllowGameplay = false;
            yield return ScreenFade.Instance.FadeOut(2f);
            yield return new WaitForSecondsRealtime(2f);

            // Save -->
            SpeechUI.Instance.Behavior.variableStorage.SetValue("$passedOutToday", false);
            TimeController.Instance.NextDay();
            Player.Instance.Stamina = Player.Instance.maxStamina;
            GameSaver.Instance.SaveGame();

            // Now we fade in. -->
            yield return ScreenFade.Instance.FadeIn(1f);
            GameplayManager.Instance.AllowGameplay = true;
        }
    }

}