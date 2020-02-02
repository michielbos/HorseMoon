using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HorseMoon {

public class SceneController : MonoBehaviour {
    public void Awake() {
        DontDestroyOnLoad(gameObject);
        if (!SceneManager.GetSceneByName("TheFarm").isLoaded)
            SceneManager.LoadScene("TheFarm", LoadSceneMode.Additive);
    }
}

}