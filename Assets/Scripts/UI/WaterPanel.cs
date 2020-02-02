using System;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.UI {

public class WaterPanel : SingletonMonoBehaviour<WaterPanel> {
    public Image indicator;

    public new void Awake() {
        base.Awake();
        SetVisible(false);
    }

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

    public void UpdateWater(float percentage) {
        indicator.fillAmount = percentage;
    }
}

}