using System;
using HorseMoon.Inventory;

namespace HorseMoon {

[Serializable]
public class GameSaverData {
    public int money;
    public int wood;
    public Item.ItemData[] itemDatas;
}

}