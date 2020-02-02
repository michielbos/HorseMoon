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
    
    [Serializable]
    public class PlantedCropData {
        public CropData cropData;
        public Vector2Int position;
        public int age;
        public bool dehydrated;
        public bool dead;

        public PlantedCropData(CropData cropData, Vector2Int position, int age, bool dehydrated, bool dead) {
            this.cropData = cropData;
            this.position = position;
            this.age = age;
            this.dehydrated = dehydrated;
            this.dead = dead;
        }
    }

    public void SetCrop(CropData cropData) {
        this.cropData = cropData;
        UpdateSprite();
    }

    public void Load(PlantedCropData data) {
        SetCrop(data.cropData);
        age = data.age;
        dehydrated = data.dehydrated;
        dead = data.dead;
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
        return !dead && cropData.IsFullyGrown(age) && player.playerController.bag.CanAdd(cropData.produce, 1);
    }

    public override void UseObject(Player player) {
        if (!CanUse(player))
            return;
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
    
    public PlantedCropData GetData() {
        return new PlantedCropData(cropData, transform.position.WorldToTile(), age, dehydrated, dead);
    }
}

}