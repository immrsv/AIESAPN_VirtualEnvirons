using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Threading;


public class SceneLoader : MonoBehaviour {

    public string BundleLocation = @"C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles/";
    public string BundleName;
    // C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles

    FileSystemWatcher watcher;

    protected bool LocationDirty = true;

    // Dictionary of Scene Names : bundle names
    protected Dictionary<string, string> Scenes = new Dictionary<string, string>();
    protected Dictionary<string, string> _Scan;

    protected Thread BackgroundScanThread;

	// Use this for initialization
	void Start () {

        BackgroundScanThread = new Thread(new ThreadStart(BackgroundScan));

        watcher = new FileSystemWatcher(BundleLocation);
        watcher.Created += Watcher_OnChanged;
        watcher.Changed += Watcher_OnChanged;
        watcher.Deleted += Watcher_OnChanged;
        watcher.Renamed += Watcher_OnRenamed;

        watcher.Filter = "*.";

        watcher.EnableRaisingEvents = true;

        if ( !Directory.Exists(BundleLocation )) {
            Debug.LogError("Bundle Location Is Invalid: " + BundleLocation);
        }

        foreach ( var name in Directory.GetFiles(BundleLocation))
        {
            //Debug.Log("FILE: " + name);
            //var file = new FileInfo(BundleLocation + "/" + name);
            //if (File)
            //Bundles.AddRange
        }
	}

    private void Watcher_OnRenamed(object sender, RenamedEventArgs e) {
        LocationDirty = true;
    }
    private void Watcher_OnChanged(object sender, System.IO.FileSystemEventArgs e) {
        LocationDirty = true;
    }

    // Update is called once per frame
    void Update () {

        if ( LocationDirty && _Scan == null && !BackgroundScanThread.IsAlive) {
            
            BackgroundScanThread.Start();
        }

        if (_Scan != null) {
            lock (_Scan) {
                Scenes = _Scan;
                _Scan = null;
            }
        }
	}

    void BackgroundScan() {
        var results = new Dictionary<string, string>();
        
        lock(_Scan) {
            _Scan = results;
        }
        LocationDirty = false;
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
