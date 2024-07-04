using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class LeafItem : DrawingItem
    {
        public override void Setup()
        {
            RegisterEventDrag();
        }
    }
}
