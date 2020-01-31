using UnityEngine;

namespace HorseMoon.Tools {

// This is not a carpenter's hammer.
public class HammerTool : Tool {
    public override bool CanUse(Player player, InteractionObject target) {
        return target.objectType == ObjectType.Rock;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (CanUse(player, target)) {
            Destroy(target.gameObject);
        }
    }
}

}