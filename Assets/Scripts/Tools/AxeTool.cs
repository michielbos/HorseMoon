using HorseMoon.Objects;
using UnityEngine;

namespace HorseMoon.Tools {

public class AxeTool : Tool {
    public override bool CanUse(Player player, InteractionObject target) {
        if (target.objectType != ObjectType.Stump)
            return false;
        TreeStump treeStump = target.GetComponent<TreeStump>();
        return treeStump != null && player.Stamina >= treeStump.staminaCost;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (!CanUse(player, target))
            return;
        TreeStump treeStump = target.GetComponent<TreeStump>();
        player.Stamina -= treeStump.staminaCost;
        treeStump.health--;
        if (treeStump.health <= 0) {
            ScoreManager.Instance.wood += treeStump.woodYield;
            Destroy(target.gameObject);
        }
    }
}

}