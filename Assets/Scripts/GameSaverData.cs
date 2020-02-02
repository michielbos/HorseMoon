using System;
using HorseMoon.Inventory;
using Objects;
using UnityEngine;

namespace HorseMoon {

[Serializable]
public class GameSaverData {
    public int money;
    public int wood;
    public Item.ItemData[] itemDatas;
    public Vector3Int[] plowedTiles;
    public CropBlocker.CropBlockerData[] cropBlockers;
    public PlantedCrop.PlantedCropData[] plantedCrops;
}

}