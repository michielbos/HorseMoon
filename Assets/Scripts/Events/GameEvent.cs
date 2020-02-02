using System;
using UnityEngine;

namespace HorseMoon {

[Serializable]
public abstract class GameEvent : MonoBehaviour {
    public bool started;
    public bool finished;
}

}