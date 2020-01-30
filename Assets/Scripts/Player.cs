using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {
    private PlayerController playerController;

    public Vector2Int TilePosition => playerController.TilePosition;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }
}

}