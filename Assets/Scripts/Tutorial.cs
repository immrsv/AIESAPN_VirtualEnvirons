using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour {
    public TextMeshProUGUI TuteText;
    public GameObject MeasuringTool;
    public GameObject RedOrb;
    public GameObject GreenOrb;
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

        switch (State) {
            case 1:
                if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
                    TuteText.text = "Good Job! Now hold in the Menu button to bring up the menu";
                break;
            case 2:
                if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                    TuteText.text = "With the menu open select 'Measurement tool' and then 'toggle tool' to enable measuring tool";
                else if (MeasuringTool.activeSelf)
                    TuteText.text = "Use the grip button to place two measuring points";
                break;
            case 3:

                break;

        }


		if(State == 2 && Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            TuteText.text = "Select Measuring Tool option";
        }
        
        if (State == 1 && MeasuringTool.activeSelf)
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
