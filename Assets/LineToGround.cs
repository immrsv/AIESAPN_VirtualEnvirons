using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToGround : MonoBehaviour {
    public LineRenderer lineToGround;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lineToGround.SetPosition(0, gameObject.transform.localPosition);
        lineToGround.SetPosition(1, new Vector3(transform.localPosition.x, 0, transform.localPosition.z));
    }
}
