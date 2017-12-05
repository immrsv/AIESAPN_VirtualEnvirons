using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Threading;
using System.Linq;

using RadialMenu.ScriptedMenus;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour {

    public string BundleLocation = @"C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles/";
    //public string BundleName;
    // C:/Users/EDATS VR/Desktop/AIE/Test Apps/Asset Bundles

    FileSystemWatcher watcher;

    protected bool LocationDirty = true;

    // Dictionary of Scene Names : bundle names
    protected Dictionary<string, string> Scenes = new Dictionary<string, string>();
    protected Dictionary<string, string> _Scan;

    protected static Dictionary<string, AssetBundle> LoadedBundles = new Dictionary<string, AssetBundle>();

    protected Thread BackgroundScanThread;

    protected bool IsScanRunning { get { return BackgroundScanThread != null && BackgroundScanThread.IsAlive; } }

    public Material DefaultSkybox;

    [Header("Radial Menu")]
    public RadialMenu.RadialMenu_Master Menu;
    protected RadialMenu_MenuItem MenuItem;

    [Header("Loading Screen")]
    public UnityEngine.UI.Image LoadingBg;
    public TMPro.TextMeshProUGUI LoadingText;

    public event System.Action<Scene> LoadedScene;
    protected void RaiseLoadedScene(Scene scene) {
        if (LoadedScene != null) LoadedScene(scene);
    }

    public event System.Action<Scene> UnloadedScene;
    protected void RaiseUnloadedScene(Scene scene) {
        if (LoadedScene != null) UnloadedScene(scene);
    }

    // Use this for initialization
    void Start() {

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

        if (!Directory.Exists(BundleLocation)) {
            Debug.LogError("Bundle Location Is Invalid: " + BundleLocation);
        }

        if (Menu != null) {
            MenuItem = ScriptableObject.CreateInstance<RadialMenu_MenuItem>();
            MenuItem.name = "Scenes...";

            Menu.RootItems.Add(MenuItem);
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Camera.main.GetComponent<Skybox>().material = DefaultSkybox;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {

    }

    private void Watcher_OnRenamed(object sender, RenamedEventArgs e) {
        LocationDirty = true;
    }
    private void Watcher_OnChanged(object sender, FileSystemEventArgs e) {
        LocationDirty = true;
    }

    // Update is called once per frame
    void Update() {

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

        foreach (var file in files) {
            if (string.IsNullOrEmpty(file)) continue;

            var scenes = ParseManifest(file);

            foreach (var scene in scenes) {
                Debug.Log("SceneLoader::BackgroundScan():\nFile[" + new FileInfo(file).Name + "] -> Scene[" + scene + "]");
                results.Add(scene, file.Replace(".manifest",""));
            }

        }

        //lock (_Scan) {
            _Scan = results;
        //}
    }


    string[] ParseManifest(string manifestPath) {
        var lines = File.ReadAllLines(manifestPath);

        var insideSceneBlock = false;

        var results = new List<string>();

        foreach (var line in lines) {
            var trimmedLine = line.Trim();

            if (!insideSceneBlock) {
                if (trimmedLine.StartsWith("Assets:"))
                    insideSceneBlock = true;
                continue;
            }
            else {
                if (!line.Trim().StartsWith("-"))
                    break;

                var virtualFile = new FileInfo(line);
                results.Add(virtualFile.Name.Replace(virtualFile.Extension, ""));
            }
        }

        return results.ToArray();
    }

    void TransferResults() {
       // lock (_Scan) {
            Scenes = _Scan;
            _Scan = null;

            if (Menu != null) {
                MenuItem.Children.Clear();

                foreach (var scene in Scenes.Keys) {
                    var item = ScriptableObject.CreateInstance<RadialMenu_MenuItem>();
                    item.name = scene;
                    item.ActionOverride = () => { LoadScene(scene); };

                    MenuItem.Children.Add(item);
                }
            }
        //}
    }

    void LoadScene(string sceneName) {

        Menu.Hide();

        LoadingBg.enabled = true;
        LoadingText.enabled = true;

        StartCoroutine(CoLoader(sceneName));
    }

    IEnumerator CoLoader(string sceneName) {

        // Unload other scenes
        if (SceneManager.sceneCount > 1) {
            Camera.main.GetComponent<Skybox>().material = DefaultSkybox;
            var oldScene = SceneManager.GetSceneAt(1);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).name);
            RaiseUnloadedScene(oldScene);
        }

        if (!Scenes.ContainsKey(sceneName)) {
            Debug.LogError("Scene Loader::LoadScene(): Request scene (" + sceneName + ") not found!");
            yield break;
        }

        Debug.Log("Loading Scene: " + Scenes[sceneName]);

        var bundleName = new FileInfo(Scenes[sceneName]).Name;

        AssetBundle bundle = null;

        if (LoadedBundles.ContainsKey(bundleName)) {
            bundle = LoadedBundles[bundleName];
        }
        else {
            var bundleRequest = AssetBundle.LoadFromFileAsync(Scenes[sceneName]);
            //bundle = AssetBundle.LoadFromFile(Scenes[sceneName]);

            while (!bundleRequest.isDone)
                yield return null;

            bundle = bundleRequest.assetBundle;
            LoadedBundles.Add(bundleName, bundle);
        }

        if (bundle == null) {
            Debug.Log("Failed to load AssetBundle! (" + Scenes[sceneName] + ")");
            yield break;
        }


        var loadRequest = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!loadRequest.isDone)
            yield return null;

        var scene = SceneManager.GetSceneByName(sceneName);
        var objects = scene.GetRootGameObjects();
        
        foreach (var obj in objects) {
            var skybox = obj.GetComponentInChildren<Skybox>();
            if (skybox != null && skybox.enabled & skybox.material != null) {

                Camera.main.GetComponent<Skybox>().material = skybox.material;
                break;
            }
        }

        RaiseLoadedScene(scene);

        LoadingBg.enabled = false;
        LoadingText.enabled = false;
    }
}
