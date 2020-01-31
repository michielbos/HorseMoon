using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {
    public PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }
}

}