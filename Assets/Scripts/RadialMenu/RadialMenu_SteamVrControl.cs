using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace RadialMenu
{
    public class RadialMenu_SteamVrControl : MonoBehaviour
    {
        public SteamVR_TrackedController TrackedController;
        
        public bool AlignToViewsphere;
        public float Distance = 20;

        protected RadialMenu_Master Menu;

        protected CursorLockMode PreviousCursorLockState;

        // Use this for initialization
        void Start()
        {
            Menu = GetComponent<RadialMenu_Master>();
            TrackedController.MenuButtonClicked += TrackedController_MenuButtonClicked;
            TrackedController.MenuButtonUnclicked += TrackedController_MenuButtonUnclicked;
        }

        private void TrackedController_MenuButtonUnclicked(object sender, ClickedEventArgs e)
        {
            Menu.Hide();
        }

        private void TrackedController_MenuButtonClicked(object sender, ClickedEventArgs e)
        {
            var position = TrackedController.transform.position;

            position += TrackedController.transform.forward * Distance;

            var rotation = Camera.main.transform.rotation;
            if (AlignToViewsphere)
            {
                Debug.LogWarning(gameObject.name + " - PieMenu_MouseControl::Update(): AlignToViewsphere Not Implemented.");
            }

            Menu.Show(position, rotation);
        }


        // Update is called once per frame
        void Update()
        {
            var position = Menu.transform.InverseTransformPoint(TrackedController.transform.position);
            var direction = Menu.transform.InverseTransformVector(TrackedController.transform.forward);

            // distance from plane
            var dist = Mathf.Abs(position.z);

            // Find ray intersect
            var intersect = position + (direction * dist);

            if (Menu.IsVisible)
            {
                Menu.UpdateCursor(intersect);
            }

        }
    }
}