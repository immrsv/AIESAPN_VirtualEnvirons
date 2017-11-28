using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadialMenu {
    public class RadialMenu_CentreAnimator : MonoBehaviour {


        public GameObject Text;
        public GameObject Rings;

        public bool IsConfirmVisible {
            get {
                return Text.activeSelf;
            }
            set {
                Text.SetActive(value);
                Rings.SetActive(value);
            }
        }
    }
}