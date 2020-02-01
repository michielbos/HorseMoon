using UnityEngine;

namespace HorseMoon.Tools {

// This is not a carpenter's hammer.
public class HammerTool : Tool {
    public override bool CanUse(Player player, InteractionObject target) {
        return target.objectType == ObjectType.Rock && player.Stamina >= 10;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (CanUse(player, target)) {
            toolObject.GetComponent<Animator>().SetTrigger("Use");
            Destroy(target.gameObject);
            player.Stamina -= 10;
        }
    }
}

}