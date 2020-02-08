using System;
using HorseMoon.Inventory;
using UnityEngine;

namespace HorseMoon {

[CreateAssetMenu(fileName = "CropData", menuName = "HorseMoon/Crop Data")]
public class CropData : ScriptableObject {
    public GrowthStage[] stages;
    public Sprite deadSprite;
    public ItemInfo produce;
    public HarvestType harvestType;

    [Serializable]
    public class GrowthStage {
        public Sprite sprite;
        public int days;
        public string sortingLayerName = "Ground";
        public int sortingOrder;
    }
    
    public enum HarvestType {
        RemovePlant,
        KillPlant,
        PreviousStage
    }

    public GrowthStage GetAgeStage(int age) {
        int neededDays = 0;
        foreach (GrowthStage stage in stages) {
            neededDays += stage.days;
            if (neededDays > age)
                return stage;
        }
        return stages[stages.Length - 1];
    }
    
    public int GetAgeForSecondLastStage() {
        int neededDays = 0;
        for (int i = 0; i < stages.Length - 2; i++) {
            neededDays += stages[i].days;
        }
        return neededDays;
    }

    public bool IsFullyGrown(int age) => GetAgeStage(age) == stages[stages.Length - 1];
}

}