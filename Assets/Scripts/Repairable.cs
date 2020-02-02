using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HorseMoon.Speech;

namespace HorseMoon.Objects
{
    public abstract class Repairable : InteractionObject
    {
        public GameObject RepairedPrefab;

        public void Repair()
        {
            Instantiate(RepairedPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}