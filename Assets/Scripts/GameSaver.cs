using System;
using UnityEngine;

namespace HorseMoon {

[Serializable]
public class GameSaver : SingletonMonoBehaviour<GameSaver> {
    private const string SaveKey = "Savegame";
    
    private GameSaverData gameSaverData;
    
    private void Update() {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) {
            LoadGame();
        }
    }

    public void SaveGame() {
        gameSaverData = new GameSaverData();
        gameSaverData.money = ScoreManager.Instance.Money;
        gameSaverData.wood = ScoreManager.Instance.wood;
        gameSaverData.itemDatas = Player.Instance.playerController.bag.GetItemDatas();
        string json = JsonUtility.ToJson(gameSaverData, false);
        Debug.Log(json);
        PlayerPrefs.SetString(SaveKey, json);
    }

    public void LoadGame() {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (json == "") {
            Debug.Log("No saved game.");
            return;
        }
        gameSaverData = JsonUtility.FromJson<GameSaverData>(json);
        ScoreManager.Instance.Money = gameSaverData.money;
        ScoreManager.Instance.wood = gameSaverData.wood;
        Player.Instance.playerController.bag.SetItemsDatas(gameSaverData.itemDatas);
        Debug.Log("Game loaded.");
    }
}

}