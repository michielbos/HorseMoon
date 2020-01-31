namespace HorseMoon.Objects {

public class Bed : InteractionObject {
    public override bool CanUse() {
        return true;
    }

    public override void UseObject() {
        TimeController.Instance.NextDay();
    }
}

}