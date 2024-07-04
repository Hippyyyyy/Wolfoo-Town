using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ShoppingBasket : BackItem
    {
        [SerializeField] Transform itemZone;
        private int curIdx;

        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, shoppingBasket = this });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;
            if (item.shoppingBasket != null) return;
            if (item.waterProvider != null) return;
            if (item.character != null) return;
            if (item.newCharacter != null) return;

            if (Vector2.Distance(item.backitem.transform.position, itemZone.position) > 2) return;

            item.backitem.JumpIntoBasket(itemZone.GetChild(curIdx));

            curIdx++;
            if (curIdx >= itemZone.childCount) curIdx = 0;
        }
    }
}