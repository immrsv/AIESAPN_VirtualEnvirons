using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlphaWithView : MonoBehaviour {
    
    public AnimationCurve BgAlphaScale;

    public AnimationCurve  FgAlphaScale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var alignment = Vector3.Dot(Camera.main.transform.forward, transform.forward);
        //Debug.Log("DOT: " + alignment);


        var bgAlpha = Mathf.Clamp01(BgAlphaScale.Evaluate(alignment));
        var fgAlpha = Mathf.Clamp01(FgAlphaScale.Evaluate(alignment));

        var images = GetComponentsInChildren<Image>();
        foreach(var image in images)
        {
            image.color = new Color(image.color.r, image.color.b, image.color.b, bgAlpha);
        }

        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            text.color = new Color(text.color.r, text.color.b, text.color.b, fgAlpha);
        }
    }
}
