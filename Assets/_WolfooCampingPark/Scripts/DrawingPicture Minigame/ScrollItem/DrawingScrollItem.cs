using SCN.UIExtend;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class DrawingScrollItem : ScrollItemBase
    {
        [SerializeField] DrawingItem drawingItem;
        protected override void Setup(int order)
        {

        }
        public void Assign(Sprite icon)
        {
            drawingItem.Setup();
            drawingItem.Assign(icon);

        }
        public void Assign(Color color)
        {
            var brushItem = drawingItem.GetComponent<BrushColorItem>();
            if(brushItem != null)
            {
                brushItem.Assign(color);
              //  if (order == 0) brushItem.Choosing();
            }
        }
    }
}
