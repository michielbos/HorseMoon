using UnityEngine;

namespace HorseMoon {

public abstract class Tool : MonoBehaviour {
    // Whatever is spawned when the player equips the tool.
    public GameObject toolPrefab;
    
    public virtual bool CanUse(Player player, InteractionObject target) {
        return false;
    }

    public virtual bool CanUse(Player player, Vector2Int target) {
        return false;
    }

    public virtual void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        
    }

    public virtual void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        
    }
}

}