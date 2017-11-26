using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTeleporting : MonoBehaviour {
    static List<NodeTeleporting> AllNodes = new List<NodeTeleporting>(); 

    public string Name;
    public Sprite Sprite; 

    //private SteamVR_Controller.Device Controller 
    //{
    //    get { return SteamVR_Controller.Input((int)trackedObj.index); }
    //}

    //void OnTriggerStay (Collider other)
    //{
    //    trackedObj = other.GetComponent<SteamVR_TrackedObject>();
    //    if (trackedObj != null && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
    //    {
    //        player.transform.position = transform.position;
    //    }
    //}

    private void Awake()
    {
        AllNodes.Add(this);
    }

    private void OnDestroy()
    {
        AllNodes.Remove(this);
    }

    public void Warp(Transform playerTransform)
    {
        playerTransform.position = transform.position;
    }
}