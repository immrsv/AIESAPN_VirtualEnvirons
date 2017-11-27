using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAlpha : MonoBehaviour {
    public AnimationCurve Fade;

    [Range(0, 1)]
    public float MaxAlpha;

    [Space]
    public Transform PlayerTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var dist = (transform.position - PlayerTransform.position).magnitude;
        var alpha = Fade.Evaluate(dist) * MaxAlpha;
        Color color = GetComponent<MeshRenderer>().material.color;
        color.a = alpha;

        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
	}
}
