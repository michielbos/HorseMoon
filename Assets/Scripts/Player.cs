using System;
using UnityEngine;
using HorseMoon.UI;
using HorseMoon.Inventory;
using HorseMoon.Inventory.UI;
using HorseMoon.Speech;

namespace HorseMoon {

    [RequireComponent(typeof(PlayerController))]
    public class Player : SingletonMonoBehaviour<Player> {
        public PlayerController playerController;
        public Bag bag;
        [SerializeField]
        private int stamina = 100;
        public int maxStamina = 100;
    
        public StaminaPanel staminaPanel;

        public int Stamina {
            get => stamina;
            set {
                stamina = value; 
                staminaPanel.UpdateStamina((float) stamina / maxStamina);
            }
        }

        public bool LockControls {
            get => lockControls;
            set {
                BagWindow.Instance.enabled = !value;
                playerController.enabled = !value;
                lockControls = value;
            }
        }
        private bool lockControls;

        private void Start() {
            playerController = GetComponent<PlayerController>();
            bag = GetComponent<Bag>();
            staminaPanel.UpdateStamina((float) stamina / maxStamina);
        }
    
        public void PassOut()
        {
            LockControls = true;
            TimeController.Instance.runWorldTime = false;
            ScreenFade.Instance.fadedOut += OnFadedOut;
            ScreenFade.Instance.FadeOut(2f);
        }

        private void OnFadedOut()
        {
            Stamina = maxStamina / 2;
            GameObject passOutMarker = GameObject.Find("PassOutMarker");
            if (passOutMarker != null)
                playerController.Teleport(passOutMarker.transform.position);
            else
                Debug.LogWarning("Sorry Bit.");

            TimeController.Instance.NextDay();
            TimeController.Instance.WorldTimeHours = 10f;
            SpeechUI.Instance.Behavior.variableStorage.SetValue("$passedOutToday", true);

            ScreenFade.Instance.fadedOut -= OnFadedOut;
            ScreenFade.Instance.fadedIn += OnFadedIn;
            ScreenFade.Instance.FadeIn(2f);
        }

        private void OnFadedIn()
        {
            ScreenFade.Instance.fadedIn -= OnFadedIn;
            LockControls = false;
            TimeController.Instance.runWorldTime = true;
            if (StoryProgress.Instance.GetBool("TenderMet"))
                SpeechUI.Instance.Behavior.StartDialogue("TenderTill.SawPassout");
        }
    }

}