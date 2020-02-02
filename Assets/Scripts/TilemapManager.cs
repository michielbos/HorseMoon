using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace HorseMoon {

public class TilemapManager : SingletonMonoBehaviour<TilemapManager> {
    private const int NumberOfRocks = 8;
    private const int NumberOfStumps = 4;

    public TileType regularSoilType;
    public TileType plowedSoilType;
    public TileType wetSoilType;
    public TileBase plowedSoil;
    public TileBase regularSoil;
    public Tilemap groundTilemap;
    public CropBlocker rockPrefab;
    public CropBlocker weedPrefab;
    public CropBlocker stumpPrefab;

    private void Start() {
        AddStartingBlockers();
    }

    public void ClearPlowedTiles() {
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            TileBase tile = groundTilemap.GetTile(pos);
            if (plowedSoilType.Contains(tile)) {
                groundTilemap.SetTile(pos, regularSoil);
            }
        }
    }
    
    public Vector3Int[] GetPlowedTiles() {
        List<Vector3Int> plowedTiles = new List<Vector3Int>();
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            TileBase tile = groundTilemap.GetTile(pos);
            if (plowedSoilType.Contains(tile)) {
                plowedTiles.Add(pos);
            }
        }
        return plowedTiles.ToArray();
    }

    public void LoadPlowedTiles(Vector3Int[] plowedTiles) {
        TileBase[] tileArray = new TileBase[plowedTiles.Length];
        for (var i = 0; i < tileArray.Length; i++) {
            tileArray[i] = plowedSoil;
        }
        groundTilemap.SetTiles(plowedTiles, tileArray);
    }

    private void AddStartingBlockers() {
        List<Vector2Int> soilTiles = new List<Vector2Int>();
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            Vector2Int tilePos = pos.ToVector2Int();
            Vector3 worldPos = tilePos.TileToWorld();
            TileBase tile = groundTilemap.GetTile(pos);
            if (regularSoilType.Contains(tile)) {
                soilTiles.Add(tilePos);
                float random = Random.value;
                if (random < 0.45) {
                    CropBlocker weed = Instantiate(weedPrefab, worldPos, Quaternion.identity);
                    weed.RegisterBlocker();
                }
            }
        }

        int counter = 0;
        soilTiles.OrderBy(tile => Random.value)
            .Take(NumberOfRocks + NumberOfStumps)
            .ForEach(tile => {
                CropBlocker blocker = CropManager.Instance.GetBlocker(tile);
                if (blocker != null) {
                    CropManager.Instance.RemoveBlocker(tile);
                    Destroy(blocker.gameObject);
                }
                Instantiate(counter++ < NumberOfRocks ? rockPrefab : stumpPrefab, tile.TileToWorld(),
                    Quaternion.identity);
            });
    }

    public void OnDayPassed() {
        // Sadly this is heavier than it should be, but the tiles that would be returned by fetching via GetTilesBlock
        // would have wrong positions.
        foreach (Vector3Int pos in groundTilemap.cellBounds.allPositionsWithin) {
            Vector2Int tilePos = pos.ToVector2Int();
            TileBase tile = groundTilemap.GetTile(pos);
            if (wetSoilType.Contains(tile)) {
                groundTilemap.SetTile(pos, plowedSoil);
            } else if (plowedSoilType.Contains(tile) && !CropManager.Instance.HasCrop(tilePos)) {
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