using UnityEngine;

namespace HorseMoon.Tools {

public class SickleTool : Tool {

    public override bool CanUse(Player player, Vector2Int target) {
        return CropManager.Instance.HasCrop(target);
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        if (CanUse(player, target)) {
            toolObject.GetComponent<Animator>().Play("ScytheSwing");
            CropManager.Instance.RemoveCrop(target);
        }
    }
    
    public override bool CanUse(Player player, InteractionObject target) {
        return target.objectType == ObjectType.Weed;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (CanUse(player, target)) {
            toolObject.GetComponent<Animator>().Play("ScytheSwing");
            Destroy(target.gameObject);
        }
    }
}

}