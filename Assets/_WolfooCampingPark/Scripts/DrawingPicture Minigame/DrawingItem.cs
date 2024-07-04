using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public abstract class DrawingItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        private Transform startParent;
        private Tweener _tween;

        public static System.Action<DrawingItem> OnBeginDragAct;
        public static System.Action<DrawingItem> OnDragAct;
        public static System.Action<DrawingItem> OnEndDragAct;
        public static System.Action<DrawingItem> OnClickAct;
        private EventTrigger trigger;

        public abstract void Setup();

        public void Assign(Sprite sprite)
        {
            icon.sprite = sprite;
            startParent = transform.parent;
        }
        private void Start()
        {
        }

        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
            RemoveEvent();
        }

        public void Stick(Transform _endParent)
        {
            var item = Instantiate(this, _endParent);
            item.transform.position = transform.position;
            item.OnSticking(null); ;

            OnBack();
        }
        protected void RegisterEventDrag()
        {
            trigger = gameObject.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        protected void RegisterEventClick()
        {
            trigger = gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        protected void RemoveEvent()
        {
            if(trigger != null) trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
        private void OnBack()
        {
            transform.SetParent(startParent);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.zero;
            _tween = transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        private void OnSticking(System.Action OnSuccess)
        {
            icon.raycastTarget = false;
            _tween = transform.DOScale(Vector3.one * 1.5f, 0.25f).OnComplete(() =>
            {
                _tween = transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
                {
                    OnSuccess?.Invoke();
                });
            });
        }
        public void Release()
        {
            transform.SetParent(startParent);
            transform.localPosition = Vector3.zero;
        }
        public void OnPointerDownDelegate(PointerEventData data)
        {
            OnClickAct?.Invoke(this);

            _tween?.Kill();
            transform.localScale = Vector3.one;
            _tween = transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 1);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragAct?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;

            OnDragAct?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragAct?.Invoke(this);
        }
    }
}
