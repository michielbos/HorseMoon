using HorseMoon.Speech;
using UnityEngine;

namespace HorseMoon {

    public class Till : NPC {
        public bool shopOpen;
        public string[] chatNodes;
        public string todayNode;

        public override bool CanUse(Player player) {
            return shopOpen;
        }

        public override void UseObject(Player player)
        {
            if (shopOpen)
                SpeechUI.Instance.Behavior.StartDialogue(todayNode);
        }

        public void HandleYarnCommand(string[] p) {
            switch (p[0]) {
                case "selectItem"
                    :
                case "buy":
                    Debug.Log(p[3]);
                    int amount = int.Parse(p[2]);
                    int price = int.Parse(p[3]);
                    BuyItem(p[1], amount, price);
                    break;
                default:
                    Debug.LogWarning("No such command for Till: " + p[0]);
                    break;
            }
        }

        private void BuyItem(string item, int amount, int price) {
            ScoreManager.Instance.Money -= price;
            Player.Instance.bag.Add(item, amount);
        }

        public void PickTodayNode()
        {
            if (SpeechUI.Instance.Behavior.variableStorage.GetValue("$passedOutToday").AsBool
            && StoryProgress.Instance.GetBool("TenderMet"))
            {
                todayNode = "TenderTill.Shop";
            }
            else
            {
                int chatProgress = StoryProgress.Instance.GetInt("TTChatProgress");
                todayNode = chatNodes[chatProgress];
            }
        }
    }

}