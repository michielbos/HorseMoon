using System;
using UnityEngine;
using HorseMoon.UI;
using HorseMoon.Inventory;

namespace HorseMoon {

[RequireComponent(typeof(PlayerController))]
public class Player : SingletonMonoBehaviour<Player> {
    public PlayerController playerController;
    public Bag bag;
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
        bag = GetComponent<Bag>();
        staminaPanel.UpdateStamina((float) stamina / maxStamina);
    }
}

}