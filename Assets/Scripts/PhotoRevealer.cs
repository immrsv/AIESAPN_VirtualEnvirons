using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoRevealer : MonoBehaviour {
    public GameObject photo;
    public GameObject UIPrompt;
    protected SteamVR_TrackedController trackedObj;
    private Renderer rend;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.controllerIndex); }
    }
    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (trackedObj != null) return;

        trackedObj = other.GetComponent<SteamVR_TrackedController>();

        if (trackedObj != null)
        {
            UIPrompt.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trackedObj != null && other.GetComponent<SteamVR_TrackedController>() == trackedObj ) {
            trackedObj = null;
            UIPrompt.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (trackedObj == null) return;

        
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (!photo.activeSelf)
            {
                rend.material.color = Color.green;
                photo.SetActive(true);
            }
            else
            {
                rend.material.color = Color.red;
                photo.SetActive(false);
            }
        }
    }
}
