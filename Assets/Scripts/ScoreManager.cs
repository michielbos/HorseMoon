using System;
using UnityEngine;
using UnityEngine.UI;

namespace HorseMoon {

// Stupid name, but this will keep track of money, wood, etc.
public class ScoreManager : SingletonMonoBehaviour<ScoreManager> {
    public Text bitsText;
    
    [SerializeField]
    private int money = 300;
    public int moneyInBin;
    public int wood;
    public int stones;

    public int Money {
        get => money;
        set {
            money = value;
            bitsText.text = $"Bits: {money}";
        }
    }

    private void Start() {
        bitsText.text = $"Bits: {money}";
    }

    public void OnDayPassed() {
        Money += moneyInBin;
        moneyInBin = 0;
    }
}

}