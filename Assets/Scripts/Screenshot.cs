using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour {
    protected SteamVR_TrackedController trackedObj;
    
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.controllerIndex); }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            ScreenCapture.CaptureScreenshot("Screenshot.png");
        }
	}
}
