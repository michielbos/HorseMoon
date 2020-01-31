using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon {

[RequireComponent(typeof(SpriteRenderer))]
public class PlantedCrop : InteractionObject {
    public Pickupable pickupablePrefab;

    private CropData cropData;
    private int age;
    private bool dehydrated;
    private bool dead;

    private CropData.GrowthStage CurrentStage => cropData.GetAgeStage(age);

    public void SetCrop(CropData cropData) {
        this.cropData = cropData;
        UpdateSprite();
    }

    public void OnNextDay(bool isWatered) {
        if (isWatered) {
            dehydrated = false;
            GrowTick();
        } else if (!dehydrated) {
            dehydrated = true;
        } else {
            dead = true;
        }
        UpdateSprite();
    }

    private void GrowTick() {
        if (cropData.IsFullyGrown(age))
            return;
        age++;
    }

    private void UpdateSprite() {
        GetComponent<SpriteRenderer>().sprite = dead ? cropData.deadSprite : CurrentStage.sprite;
    }

    public override bool CanUse(Player player) {
        // TODO: False if inventory is full.
        return !dead && cropData.IsFullyGrown(age);
    }

    public override void UseObject(Player player) {
        if (!CanUse(player))
            return;
        // TODO: Bail if inventory is full.
        player.playerController.bag.Add(cropData.produce, 1);
        switch (cropData.harvestType) {
            case CropData.HarvestType.RemovePlant:
                CropManager.Instance.RemoveCrop(this);
                break;
            case CropData.HarvestType.KillPlant:
                dead = true;
                break;
            case CropData.HarvestType.PreviousStage:
                age = cropData.GetAgeForSecondLastStage();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        UpdateSprite();
    }
}

}