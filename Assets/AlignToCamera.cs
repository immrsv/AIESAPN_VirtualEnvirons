using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCamera : MonoBehaviour {

    public bool ProjectOntoPlane;
    public Vector3 PlaneNormal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var forward = (transform.position - Camera.main.transform.position).normalized;

        if (ProjectOntoPlane)
            forward = Vector3.ProjectOnPlane(forward, PlaneNormal.normalized);

        transform.forward = forward;
	}
}
