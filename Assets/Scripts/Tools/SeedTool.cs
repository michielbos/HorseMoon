using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tools {

public class SeedTool : Tool {
    public CropData cropData;
    public PlantedCrop cropPrefab;
    public TileType soilType;
    public TileType wetSoilType;

    public override bool CanUse(Player player, Vector2Int target) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        return soilType.Contains(tile) || wetSoilType.Contains(tile);
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        if (soilType.Contains(tile) || wetSoilType.Contains(tile)) {
            PlantedCrop crop = Instantiate(cropPrefab, target + new Vector2(0.5f, 0.5f), Quaternion.identity);
            crop.SetCrop(cropData);
        }
    }
}

}