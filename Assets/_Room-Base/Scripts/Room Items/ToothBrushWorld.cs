using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ToothBrushWorld : BackItemWorld
    {
        public override void Setup()
        {
            IsCarryItem = true;
            IsDragable = true;
            base.Setup();
        }
    }
}
