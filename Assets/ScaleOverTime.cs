using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOverTime : MonoBehaviour {

    public AnimationCurve Scale;
    public AnimationCurve Alpha;
    public float Duration;

    [Range(0, 1)]
    public float CycleOffset;

    public bool Loop;
    public bool Bounce;

    protected UnityEngine.UI.Image _Image;
    public UnityEngine.UI.Image Image {  get {
            if (_Image == null)
                _Image = GetComponent<UnityEngine.UI.Image>();
            return _Image;
        } }

	// Use this for initialization
	void Start () {
	}
	

	// Update is called once per frame
	void Update () {

        var offset = Duration * CycleOffset;

        var lerpFactor = (Bounce ? Mathf.PingPong(Time.time + offset, Duration) : Mathf.Repeat(Time.time + offset, Duration)) / Duration;

        var scaleFactor = Scale.Evaluate(lerpFactor);
        var alphaFactor = Alpha.Evaluate(lerpFactor);

        if (Loop || Time.time <= Duration) {

            transform.localScale = Vector3.one * scaleFactor;

            var tempColor = Image.color;
            tempColor.a = alphaFactor;
            Image.color = tempColor;
        }

	}
}
