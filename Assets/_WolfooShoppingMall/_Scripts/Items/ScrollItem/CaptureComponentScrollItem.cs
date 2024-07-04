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
    public class CaptureComponentScrollItem : ScrollItemBase
    {
        [SerializeField] Image itemImg;

        private Vector3 startScale;
        private Tweener scaleTween;
        private bool isDragging;
        private Image image;
        private Sticker sticker;
        private bool isAttach;

        public int TopicIdx { get; private set; }
        public int Id { get; private set; }

        private void Start()
        {
            image = GetComponent<Image>();
            sticker = GetComponentInChildren<Sticker>();
            if (sticker != null)
                sticker.enabled = false;
        }
        private void OnDestroy()
        {
        }

        public void AssignItem(int _topicIdx, int id, Sprite sprite, bool isScale = false)
        {
            TopicIdx = _topicIdx;
            Id = id;
            itemImg.sprite = sprite;

            if (isScale)
            {
                var curSize = itemImg.rectTransform.sizeDelta;
                GameManager.instance.ScaleImage(itemImg, curSize.x, curSize.y);
            }
            else
            {
                itemImg.SetNativeSize();
            }
        }

        protected override void Setup(int order)
        {
            startScale = transform.localScale;

            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerDown, OnPointerDown);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerUp, OnPointerUp);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerClick, OnPointerClick);
        }

        public void OnAttach(Vector3 _endPos, Transform _endParent)
        {
            image.enabled = false;

            var item = Instantiate(sticker, _endParent);
            item.transform.position = _endPos;
            item.enabled = true;
            item.AssignDrag();

            enabled = false;
            image.enabled = true;
        }

        protected override void OnStartDragOut()
        {
            base.OnStartDragOut();
            isDragging = true;

            image.enabled = false;
        }

        private void OnPointerUp(BaseEventData arg0)
        {
            if (isAttach) return;

            image.enabled = true;

            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale, 0.3f);

            if (!isDragging) return;
            isDragging = false;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragItem { captureItem = this });
        }

        private void OnPointerDown(BaseEventData arg0)
        {
            if (isAttach) return;

            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale + Vector3.one * 0.1f, 0.3f);

        }
        private void OnPointerClick(BaseEventData arg0)
        {
            EventDispatcher.Instance.Dispatch(new EventKey.OnClickItem { captureItem = this });
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);
        }
    }
}