using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Tracker : MonoBehaviour {

    public XRNode TrackingNode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = InputTracking.GetLocalPosition(TrackingNode);
	}
}
