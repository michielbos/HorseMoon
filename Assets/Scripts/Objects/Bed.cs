namespace HorseMoon.Objects {

public class Bed : InteractionObject {
    public override bool CanUse(Player player) {
        return true;
    }

    public override void UseObject(Player player) {
        TimeController.Instance.NextDay();
        player.Stamina = player.maxStamina;
    }
}

}