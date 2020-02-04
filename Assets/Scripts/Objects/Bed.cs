using HorseMoon.Inventory.UI;

namespace HorseMoon.Objects {

    public class Bed : InteractionObject {
        public override bool CanUse(Player player) {
            return true;
        }

        public override void UseObject(Player player)
        {
            ScreenFade.Instance.fadedOut += OnFadedOut;
            ScreenFade.Instance.FadeOut(2f);

            TimeController.Instance.runWorldTime = false;
            Player.Instance.LockControls = true;
        }

        private void OnFadedOut()
        {
            ScreenFade.Instance.fadedOut -= OnFadedOut;
            ScreenFade.Instance.fadedIn += OnFadedIn;
            ScreenFade.Instance.FadeIn(1f);

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