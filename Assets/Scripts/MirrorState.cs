using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorState : MonoBehaviour {
    public GameObject[] targets;
	// Use this for initialization
	void OnEnable () {
		foreach (var target in targets) {
            target.SetActive(true);
        }
	}

    void OnDisable() {
        foreach (var target in targets) {
            target.SetActive(false);
        }
    }
}
