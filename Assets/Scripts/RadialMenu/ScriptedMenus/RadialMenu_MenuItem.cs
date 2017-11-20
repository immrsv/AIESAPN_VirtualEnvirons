using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Items/Action")]
    public class RadialMenu_MenuItem : ScriptableObject
    {
        
        public string Text;

        public List<RadialMenu_MenuItem> Items;

        public virtual void Clicked() { }
        //{
        //    var go = GameObject.Find(ObjectName);

        //    if ( go != null )
        //        go.BroadcastMessage(ActionName, Parameter, SendMessageOptions.DontRequireReceiver);
        //}
    }
}