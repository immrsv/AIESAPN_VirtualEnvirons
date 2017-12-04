using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SkyboxLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
        var objects = scene.GetRootGameObjects();

        Debug.Log("Scene Loader:: Root Count: " + objects.Length + " (Expected: " + scene.rootCount + ")");

        foreach (var obj in objects) {
            var skybox = obj.GetComponentInChildren<Skybox>();
            if (skybox != null && skybox.enabled) {

                Camera.main.GetComponent<Skybox>().material = skybox.material;

                break;
            }
        }
    }
    
}
