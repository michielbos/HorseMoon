using System;
using System.Collections;
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
    
    public GameObject moneyGainPanel;
    public AudioClip cashSound;

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
        if (moneyInBin > 0) {
            Money += moneyInBin;
            moneyGainPanel.GetComponentInChildren<Text>().text = $"+ {moneyInBin}  ";
            AudioPool.PlaySound(Player.Instance.transform.position, cashSound);
            moneyInBin = 0;
            StartCoroutine(ShowMoneyGainPanel());
        }
    }

    private IEnumerator ShowMoneyGainPanel() {
        moneyGainPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        moneyGainPanel.gameObject.SetActive(false);
    }
}

}