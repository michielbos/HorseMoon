using System;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(CharacterControl))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {
    public Tool selectedTool;
    
    private CharacterControl characterController;
    private Player player;
    
    public Vector2Int TilePosition => characterController.TilePosition;

    private void Start() {
        characterController = GetComponent<CharacterControl>();
        player = GetComponent<Player>();
    }

    private void Update() {
        if (Input.GetButtonDown("Use")) {
            if (selectedTool != null)
                selectedTool.UseTool(player);
        }
    }

    private void FixedUpdate() {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        characterController.Move(moveInput);
    }
}

}