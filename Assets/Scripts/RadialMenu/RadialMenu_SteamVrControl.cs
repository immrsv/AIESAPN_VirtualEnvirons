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

            var forward = Camera.main.transform.forward;
            if (AlignToViewsphere)
            {
                forward = (position - Camera.main.transform.position).normalized;
            }

            Menu.Show(position, forward);
        }


        // Update is called once per frame
        void Update()
        {

            // Find position and direction in local space
            var position = Menu.transform.InverseTransformPoint(TrackedController.transform.position);
            var direction = Menu.transform.InverseTransformVector(TrackedController.transform.forward);

            // Find 'elevation' of position over interaction plane, then find how many multiples of direction to zero it.
            var steps = Mathf.Abs(position.z/direction.z);
            

            // Find ray intersect
            var intersect = Vector3.ProjectOnPlane(position, Vector3.forward) + Vector3.ProjectOnPlane(direction * steps, Vector3.forward);
            

            if (Menu.Visible)
            {
                Menu.UpdateCursor(Menu.transform.TransformPoint(intersect));
            }

        }
    }
}