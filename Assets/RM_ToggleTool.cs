using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM_ToggleTool : MonoBehaviour {

	void Toggle() {
        gameObject.SetActive(!gameObject.activeSelf);
        Debug.Log("RM_ToggleTool: " + gameObject.name + " -> Active: " + gameObject.activeSelf);        
    }
}
