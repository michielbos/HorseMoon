using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.GameMenuUI
{
    public class MainPanel : GameMenuPanel
    {
        public Button ResumeButton;
        public Button MainMenuButton;
        public Slider BGMSlider;
        public Slider SFXSlider;

        public GameObject QuitPanel;
        public AudioClip TestSound;

        private new void Awake()
        {
            base.Awake();
            CancelMethod = OnClickResume;
        }

        private new void Start()
        {
            base.Start();
            ResumeButton.onClick.AddListener(OnClickResume);
            MainMenuButton.onClick.AddListener(OnClickMainMenu);
            BGMSlider.value = Volume.Music * 10f;
            BGMSlider.onValueChanged.AddListener(OnChangeBGMVolume);
            SFXSlider.value = Volume.Effects * 10f;
            SFXSlider.onValueChanged.AddListener(OnChangeSFXVolume);
        }

        public void OnClickResume() {
            Menu.Close();
        }

        public void OnClickMainMenu () {
            Menu.ChangePanel(QuitPanel);
        }

        public void OnChangeBGMVolume(float value)
        {
            float vol = value / 10f;
            Volume.Music = vol;
        }

        public void OnChangeSFXVolume(float value)
        {
            float vol = value / 10f;
            Volume.Effects = vol;
            Volume.Steps = vol;
            AudioPool.PlaySound(Player.Instance.transform.position, TestSound);
        }
    }
}