using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Box : BackItem
    {
        [SerializeField] Transform itemZone;
        private int seatMax;
        private int curSeatIdx;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();

            seatMax = itemZone.childCount;
            curSeatIdx = 0;
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.toy != null)
            {
                if (Vector2.Distance(item.toy.transform.position, itemZone.position) > 1) return;

                item.toy.transform.SetParent(curSeatIdx == 0 ? itemZone : itemZone.GetChild(curSeatIdx));
                item.toy.JumpToEndLocalPos(Vector3.zero, () =>
                {

                });
                curSeatIdx++;
                if (curSeatIdx >= seatMax) curSeatIdx = 0;
            }
        }
    }
}