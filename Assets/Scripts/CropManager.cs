using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon {

public class CropManager : SingletonMonoBehaviour<CropManager> {
    public PlantedCrop plantedCropPrefab;
    public TileType wetSoilType;

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

    public bool GetCrop(Vector2Int tile) => HasCrop(tile) ? plantedCrops[tile] : null;

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
        RemoveBlocker(cropBlockers.First(pair => pair.Value == blocker).Key);
    }
}

}