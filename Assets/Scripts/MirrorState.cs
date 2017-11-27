using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorState : MonoBehaviour {
    public GameObject[] targets;
	// Use this for initialization
	void OnEnable () {
		foreach (var target in targets) {
            if (!target) continue;
            target.SetActive(true);
        }
	}

    void OnDisable() {
        foreach (var target in targets) {
            if (!target) continue;
            target.SetActive(false);
        }
    }
}
