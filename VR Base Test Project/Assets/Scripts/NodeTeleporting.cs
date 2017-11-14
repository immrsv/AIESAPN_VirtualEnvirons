using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTeleporting : MonoBehaviour {
    
    public Transform player, node;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller 
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void OnTriggerStay (Collider other)
    {
        print("node collided");
        if (other.tag == "Controller" && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            print("node grip pressed");
            player.transform.position = node.transform.position;
        }
    }
}