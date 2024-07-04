using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Mashmallow : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            isComparePos = true;
            IsCarry = true;
            scaleIndex = 0.5f;
            IsFood = true;
        }
        protected override void Start()
        {
            base.Start();
        }

        public void AssingItem(Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();

            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
    }
}