using HorseMoon.Speech;
using UnityEngine;

namespace HorseMoon.Objects {

public class StoneStorage : InteractionObject {
    public override bool CanUse(Player player) {
        return true;
    }

    public override void UseObject(Player player) {
        SpeechUI.Instance.Behavior.ShowPopup("Current stone in storage: " + ScoreManager.Instance.stones);
    }
}

}