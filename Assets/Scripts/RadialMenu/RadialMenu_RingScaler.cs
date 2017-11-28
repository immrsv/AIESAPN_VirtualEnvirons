using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu_RingScaler : MonoBehaviour {


    public float StartScale = 1.1f;
    public float EndScale = 0.5f;

    public float Duration = 2.0f;

    protected int TweenId = -1;

    protected void OnEnable() {
        if (TweenId < 0 || !LeanTween.isTweening(TweenId))
            LeanTween.cancel(TweenId);
        
        TweenId = LeanTween.value(gameObject, (value) => { transform.localScale = Vector3.one * value; }, StartScale, EndScale,  Duration).id;
        
    }

    void OnDisable() {
        if (TweenId < 0 || !LeanTween.isTweening(TweenId))
            LeanTween.cancel(TweenId);
    }

    // Update is called once per frame
    void Update() {


    }
}
