using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace _WolfooShoppingMall
{
    public class ItemInRefrigerator : BackItem
    {
        public void AssignItem()
        {
            IsInBag = true;
            IsFood = true;
            canDrag = true;
            isComparePos = true;
            IsBeverage = true;
            IsCarry = true;
        }
        public void AssignItem(Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();

            Content = GameManager.instance.curFloorScroll.content.gameObject;
            Ground = GameManager.instance.curGround.gameObject;
        }

        protected override void InitItem()
        {
            IsInBag = true;
            IsFood = true;
            isScaleDown = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
    }
}
