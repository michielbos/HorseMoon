using System;
using HorseMoon;

namespace Objects {

public class CropBlocker : InteractionObject {

    private new void Start() {
        base.Start();
        CropManager.Instance.AddBlocker(transform.position.WorldToTile(), this);
    }

    private void OnDestroy() {
        if (CropManager.InstanceOrNull != null) {
            CropManager.InstanceOrNull.RemoveBlocker(this);
        }
    }
}

}