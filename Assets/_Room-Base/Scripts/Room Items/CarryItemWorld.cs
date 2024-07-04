using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CarryItemWorld : BackItemWorld
    {
        public override void Setup()
        {
            IsCarryItem = true;
            IsDragable = true;
            IsStandingOnTable = true;
            base.Setup();
        }
    }
}
