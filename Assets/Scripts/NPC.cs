using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
public class NPC : InteractionObject {
    [HideInInspector]
    public CharacterControl characterController;

    private new void Start() {
        base.Start();
        characterController = GetComponent<CharacterControl>();
        gameObject.SetActive(false);
    }
}

}