using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    public GameObject door;
    public Vector3 rotateOpen, rotateClose;
    bool isOpen = false;

    int TweenId = -1;

    [Space]
    [Range(1, 200)]
    public int HapticPulseCount = 20;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void OnTriggerEnter(Collider other) {

        var controller = other.GetComponent<SteamVR_TrackedObject>();

        if (!controller) return;

        StartCoroutine(PulseHaptics(SteamVR_Controller.Input((int)controller.index)));

        
    }

    IEnumerator PulseHaptics(SteamVR_Controller.Device controller) {
        for (int i = 0; i < HapticPulseCount; i++) {
            controller.TriggerHapticPulse();
            yield return new WaitForSecondsRealtime(0.001f);
        }
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