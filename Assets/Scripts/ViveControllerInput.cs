using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInput : MonoBehaviour {

    // Use this for initialization
    // 1
    private SteamVR_TrackedObject trackedObj;
    // 2
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    public GameObject UICanvas;
    public GameObject controllerObject;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update ()

    {
        // 1
        if (Controller.GetAxis() != Vector2.zero)
        {
            //Debug.Log(gameObject.name + Controller.GetAxis());
        }

        // 2
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log(gameObject.name + " Trigger Press");
        }

        // 3
        if (Controller.GetHairTriggerUp())
        {
            Debug.Log(gameObject.name + " Trigger Release");
        }

        // 4
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Press");
        }

        // 5
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            Debug.Log(gameObject.name + " Grip Release");
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            
            if(UICanvas.activeSelf == true)
            {
                
                controllerObject.GetComponent<LaserPointer>().enabled = true;
                controllerObject.GetComponent<SteamVR_LaserPointer>().enabled = false;
                UICanvas.SetActive(false);
            }
            else if(UICanvas.activeSelf != true)
            {

                controllerObject.GetComponent<LaserPointer>().enabled = false;
                controllerObject.GetComponent<SteamVR_LaserPointer>().enabled = true;
                UICanvas.SetActive(true);
            }
        }
    }

    public void Screenshot()
    {
        ScreenCapture.CaptureScreenshot("TestScreenshot.png");
        Debug.Log(System.IO.Directory.GetCurrentDirectory());
        print("screenshot should be saved");
    }
}
