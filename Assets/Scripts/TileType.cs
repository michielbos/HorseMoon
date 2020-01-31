using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon {

[CreateAssetMenu(fileName = "TileType", menuName = "HorseMoon/Tile Type")]
public class TileType : ScriptableObject {
    public TileBase[] tiles;

    public bool Contains(TileBase tile) {
        return tiles.Contains(tile);
    }
}

}