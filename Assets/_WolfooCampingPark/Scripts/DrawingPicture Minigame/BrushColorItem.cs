using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class BrushColorItem : DrawingItem
    {
        private Color myColor;

        public Color Color { get => myColor; }

        public void Assign(Color color)
        {
            myColor = color;
        }
        public override void Setup()
        {
            RegisterEventClick();
        }
    }
}
