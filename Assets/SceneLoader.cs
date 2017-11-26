using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Threading;
using System.Linq;


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

    protected bool IsScanRunning {  get { return BackgroundScanThread != null && BackgroundScanThread.IsAlive; } }

    [Header("Radial Menu")]
    public RadialMenu.RadialMenu_Master Menu;
    protected RadialMenu.ScriptedMenus.RadialMenu_MenuItem MenuItem;

	// Use this for initialization
	void Start () {

        //foreach ( var file in  Directory.GetFiles(BundleLocation, "*.manifest") ) {
        //    Debug.Log("File Dump: " + file);
        //}

        BackgroundScanThread = new Thread(new ThreadStart(BackgroundScan));
        BackgroundScanThread.Start();

        watcher = new FileSystemWatcher(BundleLocation);
        watcher.Created += Watcher_OnChanged;
        watcher.Changed += Watcher_OnChanged;
        watcher.Deleted += Watcher_OnChanged;
        watcher.Renamed += Watcher_OnRenamed;
        watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.FileName;

        watcher.Filter = "*.";

        watcher.EnableRaisingEvents = true;

        if ( !Directory.Exists(BundleLocation )) {
            Debug.LogError("Bundle Location Is Invalid: " + BundleLocation);
        }

        if (Menu != null) {
            MenuItem = Instantiate(new RadialMenu.ScriptedMenus.RadialMenu_MenuItem());
            MenuItem.name = "Scenes...";

            Menu.RootItems.Add(MenuItem);
        }
	}

    private void Watcher_OnRenamed(object sender, RenamedEventArgs e) {
        LocationDirty = true;
    }
    private void Watcher_OnChanged(object sender, FileSystemEventArgs e) {
        LocationDirty = true;
    }

    // Update is called once per frame
    void Update () {

        if (LocationDirty && !IsScanRunning) {
            BackgroundScanThread = new Thread(new ThreadStart(BackgroundScan));
            BackgroundScanThread.Start();
        }

        if (_Scan != null) {
            TransferResults();
        }
	}


    void BackgroundScan() {
        var results = new Dictionary<string, string>();
        
        LocationDirty = false;

        var files = Directory.GetFiles(BundleLocation, "*.manifest");

        foreach ( var file in files) {
            if (string.IsNullOrEmpty(file)) continue;

            var scenes = ParseManifest(file);

            foreach (var scene in scenes) {
                Debug.Log("SceneLoader::BackgroundScan():\nFile[" + new FileInfo(file).Name + "] -> Scene[" + scene + "]");
                //results.Add(scene, file);
            }
            
        }

        lock(_Scan) {
            _Scan = results;
        }
    }


    string[] ParseManifest(string manifestPath)
    {
        var lines = File.ReadAllLines(manifestPath);

        var insideSceneBlock = false;

        var results = new List<string>();

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

                var virtualFile = new FileInfo(line);
                results.Add(virtualFile.Name.Replace(virtualFile.Extension, ""));
            }
        }

        return results.ToArray();
    }

    void TransferResults() {
        lock (_Scan) {
            Scenes = _Scan;
            _Scan = null;

            if (Menu != null) {
                MenuItem.Children.Clear();

                foreach (var scene in Scenes.Keys) {
                    var item = new RadialMenu.ScriptedMenus.RadialMenu_MenuItem(() => { LoadScene(scene); });
                    item.name = scene;
                    MenuItem.Children.Add(item);
                }
            }
        }
    }

    void LoadScene( string sceneName) {

        if (!Scenes.ContainsKey(sceneName)) {
            Debug.LogError("Scene Loader::LoadScene(): Request scene (" + sceneName + ") not found!");
            return;
        }

        var bundle = AssetBundle.LoadFromFile(BundleLocation + Scenes[sceneName]);
        if (bundle == null) {
            Debug.Log("Failed to load AssetBundle! (" + Scenes[sceneName] + ")");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

    }
}
