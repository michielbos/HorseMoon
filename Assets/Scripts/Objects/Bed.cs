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

        public override void UseObject(Player player)
        {
            ScreenFade.Instance.fadedOut += OnFadedOut;
            ScreenFade.Instance.FadeOut(2f);
            FindObjectOfType<MusicPlayer>().PlaySong(sleepMusic);
            TimeController.Instance.runWorldTime = false;
            Player.Instance.LockControls = true;
        }

        private void OnFadedOut()
        {
            ScreenFade.Instance.fadedOut -= OnFadedOut;
            StartCoroutine(FadeInAfterSleep(2f));
        }

        private IEnumerator FadeInAfterSleep(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ScreenFade.Instance.fadedIn += OnFadedIn;
            ScreenFade.Instance.FadeIn(1f);

            SpeechUI.Instance.Behavior.variableStorage.SetValue("$passedOutToday", false);
            TimeController.Instance.NextDay();
            Player.Instance.Stamina = Player.Instance.maxStamina;
            GameSaver.Instance.SaveGame();
        }

        private void OnFadedIn()
        {
            ScreenFade.Instance.fadedIn -= OnFadedIn;

            TimeController.Instance.runWorldTime = true;
            Player.Instance.LockControls = false;
        }
    }

}