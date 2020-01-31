using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon {

public class TilemapManager : SingletonMonoBehaviour<TilemapManager> {
    public TileType wetSoilType;
    public TileBase plowedSoil;
    public Tilemap groundTilemap;

    public void OnDayPassed() {
        UnwaterAllTiles();
    }

    private void UnwaterAllTiles() {
        // Sadly this is heavier than it should be, but the tiles that would be returned by fetching via GetTilesBlock
        // would have wrong positions.
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = groundTilemap.GetTile(tilePos);
            if (wetSoilType.Contains(tile)) {
                groundTilemap.SetTile(tilePos, plowedSoil);
            }
        }
    }
}

}