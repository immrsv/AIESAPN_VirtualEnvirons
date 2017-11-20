using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Items/Action")]
    public class RadialMenu_Action : RadialMenu_MenuItem
    {
                public string ObjectName;
        public string ActionName;
        public string Parameter;

        public List<RadialMenu_MenuItem> Items;

        public virtual void Clicked()
        {
            var go = GameObject.Find(ObjectName);

            if ( go != null )
                go.BroadcastMessage(ActionName, Parameter, SendMessageOptions.DontRequireReceiver);
        }
    }
}