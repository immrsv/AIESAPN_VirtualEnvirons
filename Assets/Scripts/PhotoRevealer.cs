using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoRevealer : MonoBehaviour {
    public GameObject photo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Controller")
        {
            photo.SetActive(true);
        }
    }
}
