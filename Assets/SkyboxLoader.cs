using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SkyboxLoader : MonoBehaviour {

    public Material DefaultSkybox;

	// Use this for initialization
	void Start () {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;

        Camera.main.GetComponent<Skybox>().material = DefaultSkybox;
    }

    private void SceneManager_sceneUnloaded(Scene scene) {
        Camera.main.GetComponent<Skybox>().material = DefaultSkybox;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
        var objects = scene.GetRootGameObjects();

        Debug.Log("Scene Loader:: Root Count: " + objects.Length + " (Expected: " + scene.rootCount + ")");

        Camera.main.GetComponent<Skybox>().material = DefaultSkybox;

        foreach (var obj in objects) {
            var skybox = obj.GetComponentInChildren<Skybox>();
            if (skybox != null && skybox.enabled & skybox.material != null ) {

                Camera.main.GetComponent<Skybox>().material = skybox.material;
                break;
            }
        }
    }
    
}
