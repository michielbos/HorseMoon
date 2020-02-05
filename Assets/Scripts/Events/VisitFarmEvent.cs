using UnityEngine;

namespace HorseMoon {

public class VisitFarmEvent : GameEvent {
    public float visitHour = 9;
    public float leaveHour = 12;
    
    public bool reachedFarm;
    
    public EventPath visitFarmPath;

    private Till till;
    private int pathProgress;


    public void OnEnable() {
        till = EventManager.Instance.till;
        pathProgress = 0;
        reachedFarm = false;
        till.characterController.Teleport(EventManager.Instance.tillEntrance.position);
    }

    public void Update() {
        float hour = TimeController.Instance.WorldTimeHours;
        if (!reachedFarm) {
            if (till.characterController.MoveTowards(visitFarmPath.GetWaypoint(pathProgress)))
                pathProgress++;
            if (pathProgress >= visitFarmPath.NumberOfWaypoints) {
                pathProgress--;
                reachedFarm = true;
                till.shopOpen = true;
            }
        } else if (hour >= leaveHour) {
            till.shopOpen = false;
            if (till.characterController.MoveTowards(visitFarmPath.GetWaypoint(pathProgress)))
                pathProgress--;
            if (pathProgress < 0)
                Finish();
        }
    }

    public void Finish() {
        finished = true;
        till.transform.position = Vector3.up * 100f;
    }
}

}