using System;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(Rigidbody2D))]
// Because a certain built-in class already took CharacterController...
public class CharacterControl : MonoBehaviour {
    public float walkSpeed;

    private new Rigidbody2D rigidbody;

    public void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public Vector2Int TilePosition {
        get => rigidbody.position.WorldToTile();
        set => rigidbody.position = value.TileToWorld();
    }

    public void Move(Vector2 direction) {
        rigidbody.MovePosition(rigidbody.position + direction * (walkSpeed * Time.fixedDeltaTime));
    }
}

}