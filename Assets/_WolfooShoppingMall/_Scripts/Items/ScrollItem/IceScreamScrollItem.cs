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
    public class IceScreamScrollItem : ScrollItemBase
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

        public void AssignItem(int _topicIdx, int id, Sprite sprite)
        {
            TopicIdx = _topicIdx;
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

        private void OnPointerUp(BaseEventData arg0)
        {
            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale, 0.3f);

            EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { iceScreamItem = this });
        }

        private void OnPointerDown(BaseEventData arg0)
        {
            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale + Vector3.one * 0.1f, 0.3f);

        }

    }
}