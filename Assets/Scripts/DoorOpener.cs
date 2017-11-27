using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    public GameObject door;
    public Vector3 rotateOpen, rotateClose;
    bool isOpen = false;

    int TweenId = -1;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void OnTriggerStay(Collider other)
    {
        if (TweenId >= 0 && LeanTween.isTweening(TweenId)) return;

        trackedObj = other.GetComponent<SteamVR_TrackedObject>();
        if (trackedObj != null && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            
            if (isOpen == true)
            {
                TweenId = LeanTween.rotate(door, rotateClose, 1f).id;
                isOpen = !isOpen;
            }
            else
            {
                TweenId = LeanTween.rotate(door, rotateOpen, 1f).id;
                isOpen = !isOpen;
            }
            
        }
    }
}