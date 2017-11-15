using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Menu Item")]
    public class RadialMenu_MenuItem : ScriptableObject
    {

        public string Text;

        public string ObjectName;
        public string Message;
        public string Parameter;

        public List<RadialMenu_MenuItem> Items;

        public virtual void DoClick()
        {
            var go = GameObject.Find(ObjectName);

            if ( go != null )
                go.BroadcastMessage(Message, Parameter, SendMessageOptions.DontRequireReceiver);
        }
    }
}