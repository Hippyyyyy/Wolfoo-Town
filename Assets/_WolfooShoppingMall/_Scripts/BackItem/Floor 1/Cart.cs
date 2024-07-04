using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Cart : CapacityBackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            canMoveToGround = true;

        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.character != null) return;

            if (item.backitem != null)
            {
                if (Vector2.Distance(item.backitem.transform.position, transform.position) > 2) return;
                if (GameManager.instance.Is_inside(item.backitem.transform.position, limitZones))
                {
                    item.backitem.JumpToEndPos(item.backitem.transform.position,transform);
                }
            }
        }
    }
}