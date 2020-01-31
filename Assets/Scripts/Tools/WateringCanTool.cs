using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tools {

public class WateringCanTool : Tool {
    public TileType plowedSoilType;
    public TileType waterType;
    public TileBase wateredSoilTile;
    public int maxWaterLevel = 30;
    public int waterLevel;

    public override bool CanUse(Player player, Vector2Int target) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);

        return (waterType.Contains(tile) || (waterLevel > 0 && plowedSoilType.Contains(tile)));
    }

    public override bool CanUse(Player player, InteractionObject target) {
        return target.objectType == ObjectType.Well;
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);

        // Try to refill.
        if (waterType.Contains(tile)) {
            waterLevel = maxWaterLevel;
            return;
        }
        // Quit if no water left.
        if (waterLevel <= 0)
            return;
        
        waterLevel--;
        if (plowedSoilType.Contains(tile)) {
            tilemap.SetTile(position, wateredSoilTile);
            toolObject.GetComponent<Animator>().Play("WaterCanWater");
        }
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (target.objectType == ObjectType.Well) {
            waterLevel = maxWaterLevel;
        }
    }
}

}