using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Popcorn : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            IsFood = true;
            isScaleDown = true;
            IsCarry = true;

            Content = GameManager.instance.curFloorScroll.content.gameObject;
            Ground = GameManager.instance.curGround.gameObject;
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { popcorn = this, backitem = this });
        }
    }
}