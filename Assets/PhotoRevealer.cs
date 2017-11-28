using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoRevealer : MonoBehaviour {
    public GameObject photo;
    protected SteamVR_TrackedObject trackedObj;
    
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

	}

    

    void OnTriggerStay(Collider other)
    {

        trackedObj = other.GetComponent<SteamVR_TrackedObject>();
        
        if (trackedObj == null) return;

        if (trackedObj != null && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            photo.SetActive(!photo.activeSelf);
        }
    }
}
