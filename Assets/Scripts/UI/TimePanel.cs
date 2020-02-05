using System;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon.UI {

public class TimePanel : MonoBehaviour {
    public Text dayText;
    public Text timeText;
    public Color lateColor;

    private Color defaultColor;

    private void Start() {
        defaultColor = timeText.color;
    }

    public void Update() {
        int hours = (int) TimeController.Instance.WorldTimeHours;
        int minutes = (int) TimeController.Instance.WorldTimeMinutes % 60;
        // Smoothen minutes per 15
        minutes = minutes / 15 * 15;
        timeText.text = $"{hours}:{minutes:D2}";

        string weekDay = TimeController.Instance.WeekDay.ToString().Substring(0, 3).ToUpper();
        int day = TimeController.Instance.Day;
        dayText.text = $"{weekDay} {day}";

        timeText.color = hours < 23 ? defaultColor : lateColor;
    }
}

}