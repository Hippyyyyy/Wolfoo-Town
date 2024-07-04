using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Toy : BackItem
    {
        [SerializeField] bool isAssign = true;
        [SerializeField] protected Sprite transformSprite;
        [SerializeField] bool canCarry = true;
        [SerializeField] bool clickToSound;
        protected Sprite idleSprite;

        protected override void InitItem()
        {
            IsCarry = canCarry;
            if (!isAssign) return;

            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            if (transformSprite != null) idleSprite = image.sprite;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            if (transformSprite != null)
            {
                image.sprite = idleSprite;
                image.SetNativeSize();
            }
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, toy = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (!canDrag) return;
            if (transformSprite != null)
            {
                image.sprite = transformSprite;
                image.SetNativeSize();
            }
        }

        public void AssignItem(Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();
        }
        public void AssignItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            startScale = transform.localScale;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(clickToSound)
            {
                PlaySound();
            }
        }
    }
}