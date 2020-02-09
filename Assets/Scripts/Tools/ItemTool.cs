using HorseMoon.Inventory;
using HorseMoon.Inventory.ItemTypes;
using UnityEngine;

namespace HorseMoon.Tools {

    public class ItemTool : Tool
    {
        public ItemInfo itemInfo;
        public float carrotDemandMultiplier;

        public override bool CanUse(Player player, InteractionObject target)
        {
            return target.objectType == ObjectType.ShippingBin && itemInfo is FoodInfo;
        }

        public override void UseTool(Player player, InteractionObject target, GameObject toolObject)
        {
            if (target.objectType == ObjectType.ShippingBin && itemInfo is FoodInfo foodInfo)
            {
                ShipItem(player, foodInfo);
                AudioPool.PlaySound(target.transform.position, target.audioClip, target.audioVolume);
            }
        }

        private void ShipItem(Player player, FoodInfo foodInfo)
        {
            Bag bag = player.playerController.bag;
            bag.Remove(itemInfo, 1);
            int shipValue = foodInfo.shipValue;
            
            if (StoryProgress.Instance.GetInt("CarrotDemand") == 1)
            {
                shipValue = (int)(shipValue * carrotDemandMultiplier);
                StoryProgress.Instance.Set("CarrotDemandShip", StoryProgress.Instance.GetInt("CarrotDemandShip") + 1);
            }
                
            ScoreManager.Instance.moneyInBin += shipValue;
        }
    }

}