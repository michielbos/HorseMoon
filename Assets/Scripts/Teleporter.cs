using System;
using UnityEngine;

namespace HorseMoon {

public class Teleporter : MonoBehaviour {
    public Teleporter destination;

    private bool disabled;
    
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null && !disabled) {
            destination.disabled = true;
            playerController.Teleport(destination.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null) {
            disabled = false;
        }
    }
}

}