using System;
using HorseMoon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {

public class CropBlocker : InteractionObject {
    public Sprite[] sprites;

    private new void Start() {
        base.Start();
        CropManager.Instance.AddBlocker(transform.position.WorldToTile(), this);
        if (sprites.Length > 0) {
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }

    private void OnDestroy() {
        if (CropManager.InstanceOrNull != null) {
            CropManager.InstanceOrNull.RemoveBlocker(this);
        }
    }
}

}