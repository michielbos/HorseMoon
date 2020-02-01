using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(fileName = "New Animated Tile", menuName = "Tiles/Animated Tile")]
public class AnimatedTile : TileBase
{
    public Sprite[] Frames;
    public int FPS = 12;
    public Tile.ColliderType ColliderType;
    
    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        tileAnimationData.animatedSprites = Frames;
        tileAnimationData.animationSpeed = 1f * FPS;
        tileAnimationData.animationStartTime = 0f;
        return true;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if(Frames != null && Frames.Length > 0)
            tileData.sprite = Frames[0];
        tileData.colliderType = ColliderType;
    }
}
