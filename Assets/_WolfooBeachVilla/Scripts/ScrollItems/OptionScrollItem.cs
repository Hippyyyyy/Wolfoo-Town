using SCN.Common;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class OptionScrollItem : ScrollItemBase
    {
        [SerializeField] Image itemImg;

        public int Id { get; private set; }
        public CustomRoomItem RoomItemPb { get; private set; }
        public PaintingWorld PaintingItemPb { get; private set; }

        public Action<OptionScrollItem> OnBeginDrag;
        public Action<OptionScrollItem> OnEndDrag;
        public Action<OptionScrollItem> OnDrag;
        private bool canTouch = true;
        private bool isTouching;

        protected override void Setup(int order)
        {
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerUp, OnPointerUp);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerDown, OnPointerDown);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.Drag, OnPointerDrag);

            Id = order;
            CanBePulledOut = false;
            EventSelfHouseRoom.OnInitScrollItem?.Invoke(this);
        }

        public void Assign(Sprite iconSprite, CustomRoomItem itemPb)
        {
            itemImg.sprite = iconSprite;
            RoomItemPb = itemPb;
        }
        public void Assign(Sprite iconSprite, PaintingWorld itemPb)
        {
            itemImg.sprite = iconSprite;
            PaintingItemPb = itemPb;
        }

        private void OnPointerDrag(BaseEventData arg0)
        {
            if (!isTouching) return;

            OnDrag?.Invoke(this);

            scrollInfinity.CurrentState = ScrollInfinityBase.State.Break;
            EventSelfHouseRoom.OnDragScrollItem?.Invoke(this);
        }
 
        private void OnPointerDown(BaseEventData eventData)
        {
            if (!canTouch) return;
            canTouch = false;
            isTouching = true;

            OnBeginDrag?.Invoke(this);
            EventSelfHouseRoom.OnBeginDragScrollItem?.Invoke(this);

            StartCoroutine(DelayTouch());
        }
        IEnumerator DelayTouch()
        {
            yield return new WaitForSeconds(0.25f);
            canTouch = true;
        }

        private void OnPointerUp(BaseEventData eventData)
        {
            if (!isTouching) return;

            OnEndDrag?.Invoke(this);
            EventSelfHouseRoom.OnEndDragScrollItem?.Invoke(this);
        }
    }
}
