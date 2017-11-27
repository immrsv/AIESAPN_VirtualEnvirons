using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAlpha : MonoBehaviour {
    public AnimationCurve Fade;
    public Transform PlayerTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var dist = (transform.position - PlayerTransform.position).magnitude;
        var alpha = Fade.Evaluate(dist);
        Color color = gameObject.GetComponent<Renderer>().material.color;
        color.a = alpha;
        gameObject.GetComponent<Material>().SetColor("new Color", color);
	}
}
