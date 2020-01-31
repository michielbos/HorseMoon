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
        return soilType.Contains(tile);
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector3Int position = new Vector3Int(target.x, target.y, 0);
        TileBase tile = tilemap.GetTile(position);
        if (soilType.Contains(tile)) {
            tilemap.SetTile(position, plowedTile);
            toolObject.GetComponent<Animator>().Play("HoeHack");
        }
    }
}

}