using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorseMoon.Speech;

namespace HorseMoon.Objects
{
    public class BrokenWell : InteractionObject
    {
        public GameObject WellPrefab;

        public override bool CanUse(Player player) {
            return true;
        }

        public override void UseObject(Player player)
        {
            SpeechUI.Instance.Behavior.StartDialogue("Well.Broken");
        }

        public void Repair()
        {
            Instantiate(WellPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}