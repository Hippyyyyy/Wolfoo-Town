using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class FloorPaintingWorld : PaintingWorld
    {
        public override void EndDrag()
        {
            base.EndDrag();

            var floor = TriggerPaintObj as PaintingFloor;
            if (floor)
            {
                OnPainting(() =>
                {
                    if (PaintingType == Type.Sprite)
                    {
                        floor.Drawing(Sprite);
                    }
                    else
                    {
                        floor.Drawing(Color);
                    }
                    Destroy(gameObject);
                });
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
