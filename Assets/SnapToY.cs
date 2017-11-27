using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToY : MonoBehaviour {

    public Transform Other;
    public MeasurementTool Tool;

    public float SnapDistance = 0.03f;

    void OnGrabStarted() {
        
    }

    void OnGrabEnded() {

        if ( Tool.SnapToY && Mathf.Abs(transform.position.y - Other.position.y) < SnapDistance) {
            var posn = transform.position;
            posn.y = Other.position.y;

            transform.position = posn;
        }
    }
}
