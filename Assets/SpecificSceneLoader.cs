using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificSceneLoader : MonoBehaviour {

    public string Scene;

    void LoadScene() {

        UnityEngine.SceneManagement.SceneManager.LoadScene(Scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
