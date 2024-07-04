using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class PlasticStraw : BackItem
    {
        [SerializeField] Transform itemZone;
        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.straw != null)
            {
                if (Vector2.Distance(item.straw.transform.position, transform.position) > 1.5f) return;

                for (int i = 0; i < itemZone.childCount; i++)
                {
                    if (itemZone.GetChild(i).childCount > 0) continue;
                    item.straw.OnJumpToPlastic(itemZone.GetChild(i));
                    return;
                }
            }
        }
    }
}