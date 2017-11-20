using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class SceneLoader : MonoBehaviour {

    public string BundleLocation;

    protected FileSystemWatcher watcher;

    protected Dictionary<string, string> Scenes = new Dictionary<string, string>();

	// Use this for initialization
	void Start () {

        //watcher = new System.IO.FileSystemWatcher(BundleLocation);
        //watcher.Changed += Watcher_Changed;

        if ( !Directory.Exists(BundleLocation )) {
            Debug.LogError("Bundle Location Is Invalid: " + BundleLocation);
        }

        foreach ( var name in Directory.GetFiles(BundleLocation))
        {
            Debug.Log("FILE: " + name);
            //var file = new FileInfo(BundleLocation + "/" + name);
            //if (File)
            //Bundles.AddRange
        }
	}

    //private void Watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
    //{
    //    throw new System.NotImplementedException();
    //}

    // Update is called once per frame
    void Update () {
		
	}

    string[] ParseManifest(string bundleName)
    {
        var lines = File.ReadAllLines(BundleLocation + "/" + bundleName + ".manifest");

        var insideSceneBlock = false;

        foreach ( var line in lines )
        {
            var trimmedLine = line.Trim();

            if (!insideSceneBlock)
            {
                if (trimmedLine.StartsWith("Assets:"))
                    insideSceneBlock = true;
                continue;
            }
            else
            {
                if (!line.Trim().StartsWith("-"))
                    break;

                
            }
        }
        return null;
    }
}
