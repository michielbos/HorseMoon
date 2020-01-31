using HorseMoon.Inventory;
using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(SpriteRenderer))]
public class Pickupable : InteractionObject {
    public ItemInfo itemInfo;
    public int amount = 1;

    private new void Start() {
        base.Start();
        renderer.sprite = itemInfo.sprite;
    }

    public override bool CanUse(Player player) {
        // TODO: False if inventory is full.
        return true;
    }

    public override void UseObject(Player player) {
        // TODO: Cancel if inventory is full.
        player.playerController.bag.Add(itemInfo, amount);
        Destroy(gameObject);
    }
}

}