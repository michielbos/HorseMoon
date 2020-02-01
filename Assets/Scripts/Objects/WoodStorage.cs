using UnityEngine;

namespace HorseMoon.Objects {

public class WoodStorage : InteractionObject {
    public override bool CanUse(Player player) {
        return true;
    }

    public override void UseObject(Player player) {
        // TODO: Put in a popup message, when those are ready.
        Debug.Log("Current wood: " + ScoreManager.Instance.wood);
    }
}

}