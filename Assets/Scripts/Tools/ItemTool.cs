using HorseMoon.Inventory;
using HorseMoon.Inventory.ItemTypes;
using UnityEngine;

namespace HorseMoon.Tools {

    public class ItemTool : Tool
    {
        public ItemInfo itemInfo;

        public override bool CanUse(Player player, InteractionObject target)
        {
            return target.objectType == ObjectType.ShippingBin && itemInfo is FoodInfo foodInfo;
        }

        public override void UseTool(Player player, InteractionObject target, GameObject toolObject)
        {
            if (target.objectType == ObjectType.ShippingBin && itemInfo is FoodInfo foodInfo)
            {
                ShipItem(player, foodInfo);
            }
        }

        private void ShipItem(Player player, FoodInfo foodInfo)
        {
            Bag bag = player.playerController.bag;
            bag.Remove(itemInfo, 1);
            ScoreManager.Instance.moneyInBin += foodInfo.shipValue;
        }
    }

}