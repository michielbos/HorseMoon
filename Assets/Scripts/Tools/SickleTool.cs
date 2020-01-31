using UnityEngine;

namespace HorseMoon.Tools {

public class SickleTool : Tool {

    public override bool CanUse(Player player, Vector2Int target) {
        return CropManager.Instance.HasCrop(target);
    }

    public override void UseTool(Player player, Vector2Int target, GameObject toolObject) {
        if (CanUse(player, target)) {
            CropManager.Instance.RemoveCrop(target);
        }
    }
}

}