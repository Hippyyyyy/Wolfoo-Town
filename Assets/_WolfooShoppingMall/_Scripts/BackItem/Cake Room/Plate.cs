using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Plate : BackItem
    {
        [SerializeField] bool isPlatePhase3;
        protected override void InitItem()
        {
            canDrag = true;
            isScaleDown = true;
            scaleIndex = 0.5f;
            isComparePos = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, plate = this });
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (!isPlatePhase3) return;
            if (item.food != null || item.backitem.IsFood)
            {
                if (Vector2.Distance(item.food.transform.position, transform.position) < 1)
                {
                    item.food.JumpToEndPos(transform.position, transform, null, assignPriorityy);
                }
            }
        }
    }
}