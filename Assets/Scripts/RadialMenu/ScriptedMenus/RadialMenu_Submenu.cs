using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Items/Action")]
    public class RadialMenu_Submenu : RadialMenu_MenuItem
    {
        public List<RadialMenu_MenuItem> Items;

    }
}