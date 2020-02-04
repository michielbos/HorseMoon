using System;
using HorseMoon;
using HorseMoon.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {

public class CropBlocker : InteractionObject {
    public Sprite[] sprites;
    public int spriteIndex = -1;

    [Serializable]
    public class CropBlockerData {
        public ObjectType type;
        public Vector2Int position;
        public int health;
        public int spriteIndex;

        public CropBlockerData(ObjectType type, Vector2Int position, int health, int spriteIndex) {
            this.type = type;
            this.position = position;
            this.health = health;
            this.spriteIndex = spriteIndex;
        }
    }

    private new void Start() {
        base.Start();
        RegisterBlocker();
        if (spriteIndex >= 0) {
            renderer.sprite = sprites[spriteIndex];
        } else if (sprites.Length > 0) {
            spriteIndex = Random.Range(0, sprites.Length);
            renderer.sprite = sprites[spriteIndex];
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
        return new CropBlockerData(objectType, transform.position.WorldToTile(), health, spriteIndex);
    }
}

}