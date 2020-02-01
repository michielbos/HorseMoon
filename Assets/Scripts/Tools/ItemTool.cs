using HorseMoon.Inventory;
using UnityEngine;

namespace HorseMoon.Tools {

public class ItemTool : Tool {
    public ItemInfo itemInfo;

    public override bool CanUse(Player player, InteractionObject target) {
        return false;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        
    }
}

}