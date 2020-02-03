using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="New Object Tile", menuName ="Tiles/Object Tile", order =0)]
public class ObjectTile : TileBase
{
    public Sprite Sprite;
    public Tile.ColliderType colliderType;
    public GameObject Ledge;
    public Vector2 OffSet = new Vector2(.25f, -.25f);

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = Sprite;
        tileData.gameObject = Ledge;
        tileData.colliderType = colliderType;
    }


    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(go != null && OffSet != Vector2.zero)
        {
            var tileData = tilemap.GetTransformMatrix(position);
            if (tileData.rotation == Quaternion.identity)
                go.transform.position += (Vector3)OffSet;
            else
                go.transform.position += new Vector3(-OffSet.x, OffSet.y);
        }
        return base.StartUp(position, tilemap, go);
    }
}
