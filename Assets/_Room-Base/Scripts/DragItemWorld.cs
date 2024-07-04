using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class DragItemWorld : BackItemWorld
    {
        [SerializeField] bool canStandOnTable1;
        public override void Setup()
        {
            IsDragable = true;
            IsCarryItem = true;
            IsStandingOnTable = canStandOnTable1;
            base.Setup();
        }
    }
}
