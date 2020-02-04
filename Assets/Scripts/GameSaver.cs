using System;
using System.Collections;
using UnityEngine;

namespace HorseMoon {

[Serializable]
public class GameSaver : SingletonMonoBehaviour<GameSaver> {
    private const string SaveKey = "Savegame";
    
    private GameSaverData gameSaverData;

    public static bool SaveGameExists => PlayerPrefs.HasKey(SaveKey);

    public static bool loadGame;

    private IEnumerator Start() {
        yield return null;
        if (loadGame)
            yield return LoadGame();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(LoadGame());
        }
    }

    public void SaveGame() {
        gameSaverData = new GameSaverData();
        gameSaverData.money = ScoreManager.Instance.Money;
        gameSaverData.wood = ScoreManager.Instance.wood;
        gameSaverData.stones = ScoreManager.Instance.stones;
        gameSaverData.itemDatas = Player.Instance.playerController.bag.GetItemDatas();
        gameSaverData.plowedTiles = TilemapManager.Instance.GetPlowedTiles();
        gameSaverData.cropBlockers = CropManager.Instance.GetBlockerDatas();
        gameSaverData.plantedCrops = CropManager.Instance.GetCropDatas();
        string json = JsonUtility.ToJson(gameSaverData, false);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public IEnumerator LoadGame() {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (json == "") {
            Debug.Log("No saved game.");
            yield break;
        }
        gameSaverData = JsonUtility.FromJson<GameSaverData>(json);
        ScoreManager.Instance.Money = gameSaverData.money;
        ScoreManager.Instance.wood = gameSaverData.wood;
        ScoreManager.Instance.stones = gameSaverData.stones;
        Player.Instance.playerController.bag.SetItemsDatas(gameSaverData.itemDatas);
        CropManager.Instance.ClearEverything();
        TilemapManager.Instance.ClearPlowedTiles();
        TilemapManager.Instance.LoadPlowedTiles(gameSaverData.plowedTiles);
        yield return null;
        CropManager.Instance.LoadCropBlockers(gameSaverData.cropBlockers);
        CropManager.Instance.LoadPlantedCrops(gameSaverData.plantedCrops);
        Debug.Log("Game loaded.");
    }
}

}