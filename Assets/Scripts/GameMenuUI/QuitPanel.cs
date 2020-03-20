using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HorseMoon.GameMenuUI
{
    public class QuitPanel : GameMenuPanel
    {
        public Button YesButton;
        public Button NoButton;

        public GameObject MainPanel;

        private new void Awake()
        {
            base.Awake();
            CancelMethod = OnClickNo;
        }

        private new void Start()
        {
            base.Start();
            YesButton.onClick.AddListener(OnClickYes);
            NoButton.onClick.AddListener(OnClickNo);
        }

        public void OnClickYes() {
            CharacterControl.ForgetLock();
            Destroy(TimeController.Instance.gameObject); // SsssSshhhhh...
            SceneManager.LoadScene(0);
        }

        public void OnClickNo() {
            Menu.ChangePanel(MainPanel);
        }
    }
}