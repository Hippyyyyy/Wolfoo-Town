using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Basket : BackItem
    {
        [SerializeField] Transform itemZone;
        private float distance;
        private int maxItem;
        private BackItem curItem;

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
            if (item.character != null) return;

            if (item.ball != null) curItem = item.ball;
            if (item.toy != null) curItem = item.toy;

            if (curItem == null) return;

            distance = Vector2.Distance(item.backitem.transform.position, itemZone.position);
            if (distance > 2f) return;

            CheckPriority(() =>
            {
                OnCheckPos(item);
            });
        }

        void OnCheckPos(EventKey.OnEndDragBackItem item)
        {
            while (true)
            {
                for (int i = 0; i < itemZone.childCount; i++)
                {
                    if (itemZone.GetChild(i).childCount <= maxItem)
                    {
                        item.backitem.transform.SetParent(itemZone.GetChild(i));
                        item.backitem.JumpToEndLocalPos(Vector3.zero, null, DG.Tweening.Ease.Flash, 100);
                        return;
                    }
                }
                maxItem++;
            }

        }
    }
}