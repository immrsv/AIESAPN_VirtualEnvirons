using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoRevealer : MonoBehaviour {
    public GameObject photo;
    public GameObject UIPrompt;
    private SteamVR_TrackedObject trackedObj;
    private Renderer rend;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
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
        if(other.tag == "Controller")
        {
            UIPrompt.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Controller")
        {
            UIPrompt.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Controller" && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && !photo.activeSelf)
        {
            rend.material.color = Color.green;
            photo.SetActive(true);
        }
        if (other.tag == "Controller" && Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && photo.activeSelf)
        {
            rend.material.color = Color.red;
            photo.SetActive(false);
        }
    }
}
