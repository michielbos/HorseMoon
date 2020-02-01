using UnityEngine;
using UnityEngine.Tilemaps;
using HorseMoon.Inventory;

namespace HorseMoon.Tools {

public class SeedTool : Tool {
    public CropData cropData;
    public PlantedCrop cropPrefab;
    public Item seedItem;
    public TileType soilType;
    public TileType wetSoilType;

    public override bool CanUse(Player player, Vector2Int target) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        return (soilType.Contains(tile) || wetSoilType.Contains(tile)) && seedItem != null &&
               !CropManager.Instance.HasCrop(target) && player.Stamina > 0;
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        if ((soilType.Contains(tile) || wetSoilType.Contains(tile)) && seedItem != null &&
            !CropManager.Instance.HasCrop(target)) {
            CropManager.Instance.PlantCrop(target, cropData);
            seedItem.Quantity--;
            player.Stamina--;
        }
    }
}

}