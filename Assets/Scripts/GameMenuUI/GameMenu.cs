using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HorseMoon.Inventory.UI;
using HorseMoon.UI;

namespace HorseMoon.GameMenuUI
{
    public class GameMenu : SingletonMonoBehaviour<GameMenu>
    {
        private Canvas canvas;
        public RectTransform frame;

        public Vector2 BorderSize;
        public float TransitionDuration;
        public GameObject FirstPanel;

        private Coroutine changePanelHelperCoroutine;

        public bool IsOpen => canvas.enabled;

        private GameObject PanelObj {
            get { return panelObj; }
            set {
                if (panelObj != null)
                {
                    Destroy(panelObj.gameObject);
                    panelObj = null;
                    panelScript = null;
                }
                if (value != null)
                {
                    panelObj = Instantiate(value, frame, false);
                    panelScript = panelObj.GetComponent<GameMenuPanel>();
                }
            }
        }
        private GameObject panelObj;

        private GameMenuPanel panelScript;

        protected new void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
        }

        private void Update()
        {
            if (!IsOpen || panelObj == null)
                return;

            if (Input.GetButtonDown("Pause"))
                Close();
        }

        public void Open()
        {
            if (IsOpen)
                return;

            canvas.enabled = true;
            BagWindow.Instance.Visible = false;
            UICanvasController.Instance.Visible = false;
            GameplayManager.Instance.AllowGameplay = false;

            frame.sizeDelta = Vector2.zero;
            ChangePanel(FirstPanel);
        }

        public void ChangePanel(GameObject panel)
        {
            PanelObj = null;
            StartChangePanelHelper(panel);
        }

        private void StartChangePanelHelper(GameObject panel) {
            if (changePanelHelperCoroutine != null)
                StopCoroutine(changePanelHelperCoroutine);
            changePanelHelperCoroutine = StartCoroutine(ChangePanelHelper(panel));
        }

        private void StopChangePanelHelper() {
            if (changePanelHelperCoroutine != null) {
                StopCoroutine(changePanelHelperCoroutine);
                changePanelHelperCoroutine = null;
            }
        }

        private IEnumerator ChangePanelHelper(GameObject panel)
        {
            float t = 0f;
            Vector2 startSize = frame.sizeDelta;
            Vector2 targetSize = panel != null ? panel.GetComponent<GameMenuPanel>().Size + BorderSize * 2f : Vector2.zero;

            while (t < 1f)
            {
                t += Time.deltaTime / TransitionDuration;
                frame.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
                yield return null;
            }

            PanelObj = panel;
            changePanelHelperCoroutine = null;
        }

        public void Close()
        {
            if (!IsOpen)
                return;

            StartCoroutine(CloseHelper());
        }

        private IEnumerator CloseHelper()
        {
            ChangePanel(null);
            yield return changePanelHelperCoroutine;

            canvas.enabled = false;
            BagWindow.Instance.Visible = true;
            UICanvasController.Instance.Visible = true;
            GameplayManager.Instance.AllowGameplay = true;
        }
    }
}

