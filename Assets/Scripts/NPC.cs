using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
public class NPC : InteractionObject {
    [HideInInspector]
    public CharacterControl characterController;

    protected new void Start() {
        base.Start();
        characterController = GetComponent<CharacterControl>();
    }
}

}