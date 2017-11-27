using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour {
    public TextMeshProUGUI TuteText;
    public GameObject MeasuringTool;
    private int State = 0;

    public SteamVR_TrackedController trackedController;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedController.controllerIndex); }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Teach teleport

		if(State == 1 && Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            TuteText.text = "Select Measuring Tool option";
        }

        if(State == 1 && MeasuringTool.activeSelf)
        {
            TuteText.text = "Press grip button to place two measuring points";
            State = 2;
        }
        if(State == 2) //check if both points are active
        {
            TuteText.text = "Manually move the points with the Trigger";
        }
        if() //you have moved a point
        {
            TuteText.text = "Go to the door and open it"; 
        }

        if() //if door is opened
        {
            //Tell player to take a screenshot
        }
	}
}
