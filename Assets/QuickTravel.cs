using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using UnityEngine.SceneManagement;

public class QuickTravel : MonoBehaviour {

    public Transform CameraRig;
    public RadialMenu.ScriptedMenus.RadialMenu_MenuItem MenuItem;

    public string TravelTag = "QuickTravel";

    protected Dictionary<string, Transform> NamedNodes = new Dictionary<string, Transform>();

    private void Awake() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void Start() {
        RebuildTargets();
    }

    private void SceneManager_sceneUnloaded(Scene arg0) {
        RebuildTargets();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {

        RebuildTargets();
    }

    void RebuildTargets() {

        var targets = GameObject.FindGameObjectsWithTag(TravelTag);
        NamedNodes.Clear();
        MenuItem.Children.Clear();

        foreach ( var target in targets) {
            NamedNodes.Add(target.name, target.transform);

            var item = ScriptableObject.CreateInstance<RadialMenu.ScriptedMenus.RadialMenu_MenuItem>();
            item.name = target.name;
            item.ActionOverride = () => { Travel(target.name); };

            MenuItem.Children.Add(item);
        }
    }

    void Travel(string name) {

        if ( !NamedNodes.ContainsKey(name)) {
            Debug.LogError("QuickTravel Target Missing: " + name);
            return;
        }

        CameraRig.position = NamedNodes[name].position;
    }
    
}
