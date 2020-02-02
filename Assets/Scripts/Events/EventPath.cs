using UnityEngine;

namespace HorseMoon {

public class EventPath : MonoBehaviour {
    public int NumberOfWaypoints => transform.childCount;
    
    public Vector2 GetWaypoint(int index) {
        return transform.GetChild(index).position;
    }
}

}