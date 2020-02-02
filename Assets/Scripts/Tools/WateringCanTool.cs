using HorseMoon.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tools {

public class WateringCanTool : Tool {
    public TileType plowedSoilType;
    public TileType waterType;
    public TileBase wateredSoilTile;
    public int maxWaterLevel = 30;
    private int waterLevel;

    public int WaterLevel {
        get => waterLevel;
        set {
            waterLevel = value;
            WaterPanel.Instance.UpdateWater((float) waterLevel / maxWaterLevel);
        }
    }

    public override void OnEquipped() {
        WaterPanel.Instance.SetVisible(true);
        WaterPanel.Instance.UpdateWater((float) waterLevel / maxWaterLevel);
    }
    
    public override void OnUnequipped() {
        WaterPanel.Instance.SetVisible(false);
    }

    public override bool CanUse(Player player, Vector2Int target) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);

        return waterType.Contains(tile) || (WaterLevel > 0 && plowedSoilType.Contains(tile) && player.Stamina > 0);
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
            WaterLevel = maxWaterLevel;
            return;
        }
        // Quit if no water left.
        if (WaterLevel <= 0)
            return;
        
        if (plowedSoilType.Contains(tile)) {
            WaterLevel--;
            toolObject.GetComponent<Animator>().SetTrigger("Use");
            tilemap.SetTile(position, wateredSoilTile);
            player.Stamina--;
        }
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (target.objectType == ObjectType.Well) {
            WaterLevel = maxWaterLevel;
        }
    }
}

}