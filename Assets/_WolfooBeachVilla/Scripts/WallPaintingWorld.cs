using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class WallPaintingWorld : PaintingWorld
    {
        public override void EndDrag()
        {
            base.EndDrag();
            var wall = TriggerPaintObj as WallWorld;
            if (wall)
            {
                OnPainting(() =>
                {
                    if (PaintingType == Type.Sprite)
                    {
                        wall.Drawing(Sprite);
                    }
                    else
                    {
                        wall.Drawing(Color);
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
