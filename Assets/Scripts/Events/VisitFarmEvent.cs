using UnityEngine;

namespace HorseMoon {

public class VisitFarmEvent : GameEvent {
    public bool reachedFarm;
    
    public NPC till;
    public EventPath visitFarmPath;

    private int pathProgress;


    public void OnEnable() {
        pathProgress = 0;
        reachedFarm = false;
        till.gameObject.SetActive(true);
        till.characterController.Teleport(EventManager.Instance.tillEntrance.position);
    }

    public void Update() {
        int hour = (int) TimeController.Instance.WorldTimeHours;
        if (!reachedFarm) {
            if (till.characterController.MoveTowards(visitFarmPath.GetWaypoint(pathProgress)))
                pathProgress++;
            if (pathProgress >= visitFarmPath.NumberOfWaypoints) {
                pathProgress--;
                reachedFarm = true;
            }
        } else if (hour >= 12) {
            if (till.characterController.MoveTowards(visitFarmPath.GetWaypoint(pathProgress)))
                pathProgress--;
            if (pathProgress < 0) {
                finished = true;
                till.gameObject.SetActive(false);
            }
        }
    }
}

}