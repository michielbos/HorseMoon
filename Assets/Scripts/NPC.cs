using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
public class NPC : MonoBehaviour {
    [HideInInspector]
    public CharacterControl characterController;

    private void Start() {
        characterController = GetComponent<CharacterControl>();
        gameObject.SetActive(false);
    }
}

}