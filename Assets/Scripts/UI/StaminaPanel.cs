using System;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.UI {

public class StaminaPanel : MonoBehaviour {
    public Image indicator;
    public Color indicatorGreen;
    public Color indicatorYellow;
    public Color indicatorRed;
    public Color criticalBlinkColor;
    public float criticalBlinkRate;
    public float yellowStamina;
    public float redStamina;
    public float criticalStamina;

    private bool critical;
    private bool blinkState;
    private float timeSinceBlink;

    public void UpdateStamina(float stamina) {
        indicator.fillAmount = stamina;
        critical = stamina < criticalStamina;
        if (stamina < redStamina)
            indicator.color = indicatorRed;
        else if (stamina < yellowStamina)
            indicator.color = indicatorYellow;
        else
            indicator.color = indicatorGreen;
    }

    private void Update() {
        if (critical) {
            if (Time.time > timeSinceBlink + 1f / criticalBlinkRate) {
                timeSinceBlink = Time.time;
                indicator.color = blinkState ? criticalBlinkColor : indicatorRed;
                blinkState = !blinkState;
            }
        }
    }
}

}