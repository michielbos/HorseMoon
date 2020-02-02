using System;
using UnityEngine;

namespace HorseMoon {

public class EventManager : SingletonMonoBehaviour<EventManager> {
    public Till till;
    
    public Transform tillEntrance;

    public VisitFarmEvent tillVisitsFarmEvent;

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
        float hour = TimeController.Instance.WorldTimeHours;
        if (hour >= tillVisitsFarmEvent.visitHour && hour < tillVisitsFarmEvent.leaveHour) {
            currentEvent = tillVisitsFarmEvent;
        }
    }
}

}