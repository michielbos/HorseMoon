using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon.Tiles
{
    [CreateAssetMenu(fileName = "New Smart Tile", menuName = "Tiles/Smart Tile")]
    public class SmartTile : TileBase
    {
        public Sprite Corner;
        public Sprite EdgeW;
        public Sprite EdgeN;
        public Sprite SolidTile;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            SmartTile.tilemap = tilemap;
            bool NW = HasTile(position + new Vector3Int(-1, 1, 0))
                , N = HasTile(position + new Vector3Int(0, 1, 0))
                , NE = HasTile(position + new Vector3Int(1, 1, 0))
                , W = HasTile(position + new Vector3Int(-1, 0, 0))
                , E = HasTile(position + new Vector3Int(1, 0, 0))
                , SW = HasTile(position + new Vector3Int(-1, -1, 0))
                , S = HasTile(position + new Vector3Int(0, -1, 0))
                , SE = HasTile(position + new Vector3Int(1, -1, 0))
                ;

            bool flipX = false;
            bool flipY = false;

            if(W != E && N != S)
            {
                // outer corner tile
                tileData.sprite = Corner;
                if(W && S)
                {
                    flipX = true;
                }
                else if(N && E)
                {
                    flipY = true;
                }
                else if(N && W)
                {
                    flipX = true;
                    flipY = true;
                }
            }
            else if(N != S)
            {
                // vertical edge tile
                tileData.sprite = EdgeN;
                if(N)
                {
                    flipY = true;
                }
            }
            else if(W != E)
            {
                // horizontal edge tile
                tileData.sprite = EdgeW;
                if (W)
                {
                    flipX = true;
                }
            }
            else
            {
                tileData.sprite = SolidTile;
            }
            
            
            var matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, Quaternion.Euler(flipY ? 180f : 0f, flipX ? 180f : 0f, 0f), Vector3.one);
            tileData.transform = matrix;
            tileData.flags = TileFlags.LockTransform;
        }

        static ITilemap tilemap;

        bool HasTile(Vector3Int position) => tilemap.GetTile(position) == this;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    base.RefreshTile(position + new Vector3Int(x, y, 0), tilemap);
                }
            }
        }
    }
}
