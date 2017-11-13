using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    public GameObject door;
    public Vector3 rotateOpen, rotateClose;
    bool isOpen = false;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Controller" && Input.GetButtonDown("Fire1"))
        {
            if (isOpen == true)
            {
                LeanTween.rotate(door, rotateClose, 1f);
                isOpen = !isOpen;
            }
            else if (isOpen == false)
            {
                LeanTween.rotate(door, rotateOpen, 1f);
                isOpen = !isOpen;
            }
            
        }
    }
}