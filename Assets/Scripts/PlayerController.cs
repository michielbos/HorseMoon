using System;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
public class PlayerController : MonoBehaviour {
    private CharacterControl characterController;

    private void Awake() {
        characterController = GetComponent<CharacterControl>();
    }

    private void FixedUpdate() {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        characterController.Move(moveInput);
    }
}

}