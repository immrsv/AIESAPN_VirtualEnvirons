using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    public GameObject door;
    public Vector3 rotateOpen, rotateClose;
    bool isOpen = false;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Controller" && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (isOpen == true)
            {
                LeanTween.rotate(door, rotateClose, 1f);
                isOpen = !isOpen;
            }
            else if (isOpen == false)
            {
                LeanTween.rotate(door, rotateOpen, 1f);
                isOpen = !isOpen;
            }
            
        }
    }
}