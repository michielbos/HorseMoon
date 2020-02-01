using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tools {

public class HoeTool : Tool {
    public TileType soilType;
    public TileBase plowedTile;

    public override bool CanUse(Player player, Vector2Int target) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        return soilType.Contains(tile) && !CropManager.Instance.HasBlocker(target) && player.Stamina > 0;
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        if (!CanUse(player, target))
            return;
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        tilemap.SetTile(position, plowedTile);
        toolObject.GetComponent<Animator>().SetTrigger("Use");
        player.Stamina--;
    }
}

}