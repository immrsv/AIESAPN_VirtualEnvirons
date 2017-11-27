using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RadialMenu;
using RadialMenu.ScriptedMenus;

public class RadialMenuTest : MonoBehaviour {


    public RadialMenu_Master Menu;

    public RadialMenu_MenuItem RootItem;

	// Use this for initialization
	void Start () {

        if (Menu != null)
        {
            RootItem = ScriptableObject.CreateInstance<RadialMenu_MenuItem>();
            RootItem.name = "Test...";

            Menu.RootItems.Add(RootItem);
        }

        BuildSayings();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Say( string saying )
    {
        Debug.Log("Simon Says, \"" + saying + "\"");
    }

    void BuildSayings()
    {
        var sayings = new string[] { "Yes", "No", "Maybe", "Banana" };

        foreach ( var saying in sayings )
        {
            var item = ScriptableObject.CreateInstance<RadialMenu_MenuItem>();

            item.name = saying;
            item.ActionOverride = () => { Say(saying); };


            RootItem.Children.Add(item);
        }
    }
}
