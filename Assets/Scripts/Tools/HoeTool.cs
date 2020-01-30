using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tools {

public class HoeTool : Tool {
    public TileType soilType;
    public TileBase plowedTile;
    
    public override void UseTool(Player player) {
        Tilemap tilemap = TilemapManager.Instance.groundTilemap;
        Vector2Int tilePosition = player.TilePosition;
        Vector3Int position = new Vector3Int(tilePosition.x, tilePosition.y, 0);
        TileBase tile = tilemap.GetTile(position);
        if (soilType.tiles.Contains(tile)) {
            tilemap.SetTile(position, plowedTile);
        }
    }
}

}