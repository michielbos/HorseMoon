using System;
using HorseMoon;
using HorseMoon.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {

public class CropBlocker : InteractionObject {
    public Sprite[] sprites;

    [Serializable]
    public class CropBlockerData {
        public ObjectType type;
        public Vector2Int position;
        public int health;

        public CropBlockerData(ObjectType type, Vector2Int position, int health) {
            this.type = type;
            this.position = position;
            this.health = health;
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

    public CropBlockerData GetData() {
        int health = 1;
        if (objectType == ObjectType.Rock)
            health = GetComponent<Rock>().health;
        else if (objectType == ObjectType.Stump)
            health = GetComponent<TreeStump>().health;
        return new CropBlockerData(objectType, transform.position.WorldToTile(), health);
    }
}

}