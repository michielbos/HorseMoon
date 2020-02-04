using System;
using System.Collections.Generic;
using System.Linq;
using HorseMoon.Objects;
using Objects;
using UnityEngine;
using UnityEngine.Tilemaps;
using static HorseMoon.PlantedCrop;
using static Objects.CropBlocker;

namespace HorseMoon {

public class CropManager : SingletonMonoBehaviour<CropManager> {
    public PlantedCrop plantedCropPrefab;
    public TileType wetSoilType;
    public CropBlocker rockPrefab;
    public CropBlocker weedPrefab;
    public CropBlocker stumpPrefab;

    private Dictionary<Vector2Int, PlantedCrop> plantedCrops = new Dictionary<Vector2Int, PlantedCrop>();
    private Dictionary<Vector2Int, CropBlocker> cropBlockers = new Dictionary<Vector2Int, CropBlocker>();

    public void PlantCrop(Vector2Int tile, CropData cropData) {
        if (plantedCrops.ContainsKey(tile)) {
            Debug.LogWarning($"Tile {tile} already has a crop!");
            return;
        }
        PlantedCrop crop = Instantiate(plantedCropPrefab, tile + new Vector2(0.5f, 0.5f), Quaternion.identity);
        plantedCrops[tile] = crop;
        crop.SetCrop(cropData);
    }

    public bool HasCrop(Vector2Int tile) => plantedCrops.ContainsKey(tile);

    public bool HasBlocker(Vector2Int tile) => cropBlockers.ContainsKey(tile);

    public PlantedCrop GetCrop(Vector2Int tile) => HasCrop(tile) ? plantedCrops[tile] : null;

    public CropBlocker GetBlocker(Vector2Int tile) => HasBlocker(tile) ? cropBlockers[tile] : null;

    public void OnDayPassed() {
        plantedCrops.ForEach(pair => {
            TileBase tile = TilemapManager.Instance.groundTilemap.GetTile(pair.Key.ToVector3Int());
            pair.Value.OnNextDay(wetSoilType.Contains(tile));
        });
    }

    public void RemoveCrop(Vector2Int tile) {
        if (!HasCrop(tile))
            return;
        Destroy(plantedCrops[tile].gameObject);
        plantedCrops.Remove(tile);
    }

    public void RemoveCrop(PlantedCrop crop) {
        RemoveCrop(plantedCrops.First(pair => pair.Value == crop).Key);
    }

    public void AddBlocker(Vector2Int tile, CropBlocker blocker) {
        if (cropBlockers.ContainsKey(tile)) {
            Debug.LogWarning($"Tile {tile} is already blocked!");
            return;
        }
        cropBlockers[tile] = blocker;
    }

    public void RemoveBlocker(Vector2Int tile) {
        cropBlockers.Remove(tile);
    }

    public void RemoveBlocker(CropBlocker blocker) {
        KeyValuePair<Vector2Int, CropBlocker> item = cropBlockers.FirstOrDefault(pair => pair.Value == blocker);
        if (item.Value != null) {
            RemoveBlocker(item.Key);
        }
    }

    public void ClearEverything() {
        foreach (PlantedCrop crop in plantedCrops.Values) {
            Destroy(crop.gameObject);
        }
        foreach (CropBlocker blocker in cropBlockers.Values) {
            Destroy(blocker.gameObject);
        }
    }

    public CropBlockerData[] GetBlockerDatas() =>
        cropBlockers.Values.Select(blocker => blocker.GetData()).ToArray();

    public void LoadCropBlockers(CropBlockerData[] cropBlockerDatas) {
        foreach (CropBlockerData blockerData in cropBlockerDatas) {
            switch (blockerData.type) {
                case ObjectType.Rock:
                    CropBlocker rock = Instantiate(rockPrefab, blockerData.position.TileToWorld(), Quaternion.identity);
                    rock.GetComponent<Rock>().health = blockerData.health;
                    break;
                case ObjectType.Stump:
                    CropBlocker stump = Instantiate(stumpPrefab, blockerData.position.TileToWorld(), Quaternion.identity);
                    stump.GetComponent<TreeStump>().health = blockerData.health;
                    break;
                case ObjectType.Weed:
                    Instantiate(weedPrefab, blockerData.position.TileToWorld(), Quaternion.identity);
                    break;
                default:
                    Debug.LogWarning("Can't load blocker as type: " + blockerData.type);
                    break;
            }
        }
    }

    public PlantedCropData[] GetCropDatas() => 
        plantedCrops.Values.Select(crop => crop.GetData()).ToArray();

    public void LoadPlantedCrops(PlantedCropData[] plantedCropDatas) {
        foreach (PlantedCropData cropData in plantedCropDatas) {
            PlantedCrop crop = Instantiate(plantedCropPrefab, cropData.position.TileToWorld(), Quaternion.identity);
            plantedCrops[cropData.position] = crop;
            crop.Load(cropData);
        }
    }
}

}