using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace HorseMoon {

[RequireComponent(typeof(Rigidbody2D))]
// Because a certain built-in class already took CharacterController...
public class CharacterControl : MonoBehaviour {
    private const float DestinationThreshold = 0.1f; 
    
    public float walkSpeed;
    public Vector2? moveDestination;

    private new Rigidbody2D rigidbody;
    public Vector2 CurrentSpeed { get; private set; }

    public void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public Vector2Int TilePosition {
        get => rigidbody.position.WorldToTile();
        set => rigidbody.position = value.TileToWorld();
    }

    private void FixedUpdate() {
        if (moveDestination.HasValue && moveDestination != rigidbody.position) {
            Vector2 direction = (moveDestination.Value - (Vector2) transform.position).normalized;
            Move(direction);
            if (Vector2.Distance(rigidbody.position, moveDestination.Value) < DestinationThreshold)
                moveDestination = null;
        }
    }

    public void Move(Vector2 direction) {
        rigidbody.MovePosition(rigidbody.position + direction * (walkSpeed * Time.fixedDeltaTime));
        CurrentSpeed = direction * walkSpeed;
    }

    public bool MoveTowards(Vector2 destination) {
        if (Vector2.Distance(rigidbody.position, destination) < DestinationThreshold)
            return true;
        moveDestination = destination;
        return false;
    }

    public void Teleport(Vector2 position) {
        rigidbody.position = position;
    }
}

}