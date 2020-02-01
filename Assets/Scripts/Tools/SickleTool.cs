using UnityEngine;

namespace HorseMoon.Tools {

public class SickleTool : Tool {

    public override bool CanUse(Player player, Vector2Int target) {
        return CropManager.Instance.HasCrop(target) && player.Stamina > 0;
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        if (CanUse(player, target)) {
            toolObject.GetComponent<Animator>().Play("ScytheSwing");
            CropManager.Instance.RemoveCrop(target);
            player.Stamina--;
        }
    }
    
    public override bool CanUse(Player player, InteractionObject target) {
        return target.objectType == ObjectType.Weed && player.Stamina > 0;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (CanUse(player, target)) {
            toolObject.GetComponent<Animator>().Play("ScytheSwing");
            Destroy(target.gameObject);
            player.Stamina--;
        }
    }
}

}