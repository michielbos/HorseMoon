using System;
using HorseMoon;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {

public class CropBlocker : InteractionObject {
    public Sprite[] sprites;

    [Serializable]
    public class CropBlockerData {
        public ObjectType type;
        public Vector2Int position;

        public CropBlockerData(ObjectType type, Vector2Int position) {
            this.type = type;
            this.position = position;
        }
    }

    private new void Start() {
        base.Start();
        RegisterBlocker();
        if (sprites.Length > 0) {
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
    
    public void RegisterBlocker() {
        Vector2Int tile = transform.position.WorldToTile();
        if (CropManager.Instance && CropManager.Instance.GetBlocker(tile) != this) {
            CropManager.Instance.AddBlocker(tile, this);            
        }
    }

    private void OnDestroy() {
        if (CropManager.InstanceOrNull != null) {
            CropManager.InstanceOrNull.RemoveBlocker(this);
        }
    }

    public CropBlockerData GetData() => new CropBlockerData(objectType, transform.position.WorldToTile());
}

}