using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Items/Action")]
    public abstract class RadialMenu_MenuItem : ScriptableObject
    {
        [Header("Interface Options")]
        public string Name;
        public UnityEngine.UI.Image Icon;

        [Header("Actions")]
        public string ObjectName;
        public string ActionName;
        public string Parameter;

        [Header("Submenu")]
        public List<RadialMenu_MenuItem> Children;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if has submenu</returns>
        public bool Clicked() {

            if (!(string.IsNullOrEmpty(ObjectName) || string.IsNullOrEmpty(ActionName))) {
                var go = GameObject.Find(ObjectName);

                if (go != null)
                    go.BroadcastMessage(ActionName, Parameter, SendMessageOptions.DontRequireReceiver);
            }

            return Children.Count > 0;

        }


    }
}