using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour {

    public float StartScale;
    public float EndScale;
    public float Duration;

    public bool Loop;
    //public bool Bounce;

	// Use this for initialization
	void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {

        var factor = Mathf.Repeat(Time.time, Duration) / Duration;

        var value = Mathf.Lerp(StartScale, EndScale, factor);
	}
}
