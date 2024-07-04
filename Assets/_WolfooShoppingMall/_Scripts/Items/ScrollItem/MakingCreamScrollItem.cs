using DG.Tweening;
using SCN;
using SCN.Common;
using SCN.UIExtend;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class MakingCreamScrollItem : ScrollItemBase
    {
        [SerializeField] Image itemImg;

        private Vector3 startScale;
        private Tweener scaleTween;

        public int TopicIdx { get; private set; }
        public int Id { get; private set; }

        private void Start()
        {
        }
        private void OnDestroy()
        {
        }

        public void AssignItem(int id, Sprite sprite)
        {
            Id = id;
            itemImg.sprite = sprite;
            itemImg.SetNativeSize();
        }

        protected override void Setup(int order)
        {
            startScale = transform.localScale;

            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerDown, OnPointerDown);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerUp, OnPointerUp);
        }

        protected override void OnDragOut()
        {
            base.OnDragOut();
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragItem { makingCream = this, direction = Direction.Down });
        }

        private void OnPointerUp(BaseEventData arg0)
        {
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragItem { makingCream = this, direction = Direction.Up });
        }

        private void OnPointerDown(BaseEventData arg0)
        {

        }

    }
}