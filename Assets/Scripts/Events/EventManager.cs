using System;
using UnityEngine;

namespace HorseMoon {

public class EventManager : SingletonMonoBehaviour<EventManager> {
    public NPC till;
    
    public Transform tillEntrance;

    public GameEvent tillVisitsFarmEvent;

    public GameEvent currentEvent;

    public void Update() {
        if (currentEvent != null) {
            if (!currentEvent.started) {
                currentEvent.started = true;
                currentEvent.enabled = true;
            }
            if (currentEvent.finished) {
                currentEvent.started = false;
                currentEvent.finished = false;
                currentEvent.enabled = false;
                currentEvent = null;
            }
            return;
        }
        SearchForNewEvent();
    }

    private void SearchForNewEvent() {
        int hour = (int) TimeController.Instance.WorldTimeHours;
        int minutes = (int) TimeController.Instance.WorldTimeMinutes % 60;
        if (hour >= 10 && hour < 12) {
            currentEvent = tillVisitsFarmEvent;
        }
    }
}

}