using System;
using Objects;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace HorseMoon {

public class TilemapManager : SingletonMonoBehaviour<TilemapManager> {
    public TileType regularSoilType;
    public TileType plowedSoilType;
    public TileType wetSoilType;
    public TileBase plowedSoil;
    public TileBase regularSoil;
    public Tilemap groundTilemap;
    public CropBlocker rockPrefab;
    public CropBlocker weedPrefab;

    private void Start() {
        AddStartingBlockers();
    }

    private void AddStartingBlockers() {
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            Vector3 worldPos = pos.ToVector2Int().TileToWorld();
            TileBase tile = groundTilemap.GetTile(pos);
            if (regularSoilType.Contains(tile)) {
                float random = Random.value;
                if (random < 0.05f) {
                    Instantiate(rockPrefab, worldPos, Quaternion.identity);
                } else if (random < 0.45) {
                    Instantiate(weedPrefab, worldPos, Quaternion.identity);
                }
            }
        }
    }

    public void OnDayPassed() {
        // Sadly this is heavier than it should be, but the tiles that would be returned by fetching via GetTilesBlock
        // would have wrong positions.
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            Vector2Int tilePos = pos.ToVector2Int();
            TileBase tile = groundTilemap.GetTile(pos);
            if (wetSoilType.Contains(tile)) {
                groundTilemap.SetTile(pos, plowedSoil);
            } else if (plowedSoilType.Contains(tile)) {
                if (Random.value < 0.3f) {
                    groundTilemap.SetTile(pos, regularSoil);
                }
            } else if (regularSoilType.Contains(tile) && !CropManager.Instance.HasBlocker(tilePos)) {
                if (Random.value < 0.01f) {
                    Instantiate(weedPrefab, tilePos.TileToWorld(), Quaternion.identity);
                }
            }
        }
    }
}

}