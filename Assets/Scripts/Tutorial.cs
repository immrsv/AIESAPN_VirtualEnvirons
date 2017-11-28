using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour {
    public TextMeshProUGUI TuteText;
    public GameObject MeasuringTool;
    public GameObject Door;
    public GameObject RedOrb;
    public GameObject GreenOrb;

    private Vector3 DoorLocation;
    private int State = 0;

    public SteamVR_TrackedController trackedController;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedController.controllerIndex); }
    }

    // Use this for initialization
    void Start () {
        DoorLocation = Door.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        switch (State) {
            case 1:
                if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
                    TuteText.text = "Good Job! Now hold in the Menu button to bring up the menu";
                break;
            case 2:
                if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                    TuteText.text = "With the menu open select 'Measurement tool' and then select 'turn on'";
                else if (MeasuringTool.activeSelf)
                    TuteText.text = "Use the grip button to place two measuring points";
                break;
            case 3:
                if (GreenOrb.activeSelf && RedOrb.activeSelf)
                    TuteText.text = "You can also move the points by going up to them and picking them up with a trigger. \n\n Next go to the door and open it";
                break;
            case 4:
                if (Door.transform.position != DoorLocation)
                    TuteText.text = "That is all of the base functionality! Open the menu and select a different scene to load it up to complete the tutorial";
                break;

        }
	}
}
