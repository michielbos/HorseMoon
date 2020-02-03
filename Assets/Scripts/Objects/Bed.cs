using HorseMoon.Inventory.UI;

namespace HorseMoon.Objects {

    public class Bed : InteractionObject {
        public override bool CanUse(Player player) {
            return true;
        }

        public override void UseObject(Player player)
        {
            FadeOut.Instance.fadedOut += OnFadedOut;
            FadeOut.Instance.Begin();
            TimeController.Instance.runWorldTime = false;
            BagWindow.Instance.enabled = false;
            Player.Instance.playerController.enabled = false;
        }

        private void OnFadedOut()
        {
            FadeOut.Instance.fadedOut -= OnFadedOut;
            FadeOut.Instance.fadedIn += OnFadedIn;

            TimeController.Instance.NextDay();
            Player.Instance.Stamina = Player.Instance.maxStamina;
            GameSaver.Instance.SaveGame();
        }

        private void OnFadedIn()
        {
            FadeOut.Instance.fadedIn -= OnFadedIn;

            TimeController.Instance.runWorldTime = true;
            BagWindow.Instance.enabled = true;
            Player.Instance.playerController.enabled = true;
        }
    }

}