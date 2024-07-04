using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class GlassOfWaterWorld : BackItemWorld, WaterProviderWorld
    {
        [SerializeField] SpriteRenderer water;
        public void Pour()
        {
        }
        public override void Setup()
        {
            IsCarryItem = true;
            IsDragable = true;
            base.Setup();
        }

    }
}
