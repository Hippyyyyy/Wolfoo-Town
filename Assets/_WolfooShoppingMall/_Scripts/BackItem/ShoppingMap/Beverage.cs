using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Beverage : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            IsBeverage = true;
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

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
    }
}