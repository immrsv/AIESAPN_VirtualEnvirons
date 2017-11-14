using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Tracker : MonoBehaviour {

    public XRNode TrackingNode = XRNode.LeftHand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = InputTracking.GetLocalPosition(TrackingNode);
        transform.localRotation = InputTracking.GetLocalRotation(TrackingNode);
	}
}
