using UnityEngine;
using HorseMoon.Objects;

namespace HorseMoon.Tools {

// This is not a carpenter's hammer.
public class HammerTool : Tool {
    public override bool CanUse(Player player, InteractionObject target)
    {
        Rock rock = target.GetComponent<Rock>();
        return target.objectType == ObjectType.Rock && player.Stamina >= rock.staminaCost;
    }

    public override void UseTool(Player player, InteractionObject target, GameObject toolObject) {
        if (CanUse(player, target))
        {
            toolObject.GetComponent<Animator>().SetTrigger("Use");
            Rock rock = target.GetComponent<Rock>();
            player.Stamina -= rock.staminaCost;
            
            rock.health--;
            if (rock.health <= 0)
                Destroy(rock.gameObject);
        }
    }
}

}