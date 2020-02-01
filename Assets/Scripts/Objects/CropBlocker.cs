using System;
using HorseMoon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {

public class CropBlocker : InteractionObject {
    public Sprite[] sprites;

    private new void Start() {
        base.Start();
        RegisterBlocker();
        if (sprites.Length > 0) {
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
    
    public void RegisterBlocker() {
        Vector2Int tile = transform.position.WorldToTile();
        if (CropManager.Instance.GetBlocker(tile) != this) {
            CropManager.Instance.AddBlocker(tile, this);            
        }
    }

    private void OnDestroy() {
        if (CropManager.InstanceOrNull != null) {
            CropManager.InstanceOrNull.RemoveBlocker(this);
        }
    }
}

}