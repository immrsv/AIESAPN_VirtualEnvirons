using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnStart : MonoBehaviour {

    public string BundleLocation = @"C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles/";
    public string BundleName;
    // C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles

    System.IO.FileSystemWatcher watcher;

    // Use this for initialization
    void Start () {
        if (!System.IO.File.Exists(BundleLocation + BundleName)) {
            Debug.LogError("LoadOnStart:: File Not Found: " + BundleLocation + BundleName);
            return;
        }

        var bundle = AssetBundle.LoadFromFile(BundleLocation + BundleName);
        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var scene = bundle.GetAllScenePaths();

        if (scene.Length == 0)
            Debug.LogError("Failed to acquire Scene list!!");
        else
            SceneManager.LoadScene(scene[0], LoadSceneMode.Additive);

        //var prefab = bundle.LoadAsset<GameObject> ("MyObject");
        //Instantiate(prefab);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
