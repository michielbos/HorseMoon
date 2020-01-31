using UnityEngine;

namespace HorseMoon {

public class InteractionObject : MonoBehaviour {
    public ObjectType objectType;
    private Color markedColor = Color.cyan;
    
    private new SpriteRenderer renderer;
    private Color spriteColor;
    private bool targeted;

    private void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }

    public virtual bool CanUse() {
        return false;
    }

    public virtual void UseObject() {
        // Replace for additional use behaviour.
        // This is only called if the tool claims it can't use this object.
    }

    public void MarkTargeted(bool targeted) {
        if (this.targeted) {
            if (!targeted) {
                renderer.color = spriteColor;
            }
        } else if (targeted) {
            spriteColor = renderer.color;
            renderer.color = markedColor;
        }
        this.targeted = targeted;
    }
}

public enum ObjectType {
    Misc,
    ShippingBin,
    Well,
    Rock,
    Stump,
    Weed
}

}