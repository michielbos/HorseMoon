using UnityEngine;

namespace HorseMoon {

public abstract class Tool : MonoBehaviour {
    public virtual bool CanUse(Player player, InteractionObject target) {
        return false;
    }

    public virtual bool CanUse(Player player, Vector2Int target) {
        return false;
    }

    public virtual void UseTool(Player player, InteractionObject target) {
        
    }

    public virtual void UseTool(Player player, Vector2Int target) {
        
    }
}

}