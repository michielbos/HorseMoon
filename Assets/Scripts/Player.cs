using System;
using HorseMoon.UI;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {
    public PlayerController playerController;
    [SerializeField]
    private int stamina = 100;
    public int maxStamina = 100;
    
    public StaminaPanel staminaPanel;

    public int Stamina {
        get => stamina;
        set {
            stamina = value; 
            staminaPanel.UpdateStamina((float) stamina / maxStamina);
        }
    }

    private void Start() {
        playerController = GetComponent<PlayerController>();
        staminaPanel.UpdateStamina((float) stamina / maxStamina);
    }
}

}