using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HorseMoon {

public class MainMenu : MonoBehaviour {
    public Button newGameButton;
    public Button loadGameButton;
    
    private void Start() {
        bool hasSaveGame = GameSaver.SaveGameExists;
        loadGameButton.interactable = hasSaveGame;
        if (hasSaveGame)
            loadGameButton.Select();
        else {
            newGameButton.Select();
        }
    }

    public void StartNewGame() {
        GameSaver.loadGame = false;
        SceneManager.LoadScene("BaseScene");
    }

    public void LoadGame() {
        GameSaver.loadGame = true;
        SceneManager.LoadScene("BaseScene");
    }

    public void Quit() {
        Application.Quit();
    }
}

}