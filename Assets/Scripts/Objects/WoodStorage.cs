using HorseMoon.Speech;
using UnityEngine;

namespace HorseMoon.Objects {

public class WoodStorage : InteractionObject {
    public override bool CanUse(Player player) {
        return true;
    }

    public override void UseObject(Player player) {
        Debug.Log("Testing lumber storage.");
        SpeechUI.Instance.Behavior.ShowPopup("Current lumber in storage: " + ScoreManager.Instance.wood);
    }
}

}