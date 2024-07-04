using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using SCN;

namespace _WolfooShoppingMall
{
    public class CharacterScrollItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Button lockBtn;
        [SerializeField] Image image;

        private int id;
        private Vector3 startPos;
        private int startSiblingIdx;
        private float distance;
        private ParticleSystem rainbowFx;
        private bool canDrag;
        private Transform startParent;
        private Tweener punchScaleTween;
        private Character character;
        private Vector3 startScale;
        private DataStorage localData;
        private EventTrigger trigger;

        private void RemoveAllEvent()
        {
            if (trigger != null && trigger.triggers.Count > 0)
                trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
        private void RegisterEvent()
        {
            trigger = gameObject.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        private void OnDestroy()
        {
            RemoveAllEvent();
        }
        private void Start()
        {
            lockBtn.onClick.AddListener(OnUnlockItem);
            startSiblingIdx = transform.GetSiblingIndex();
        }
        private void OnEnable()
        {
            image.gameObject.SetActive(true);
            image.enabled = true;
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
        }
        private void OnDisable()
        {
            image.gameObject.SetActive(false);
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);
        }

        private void GetWatchAds(EventKey.OnWatchAds obj)
        {
            if (GetInstanceID() != obj.instanceID) return;

            DataSceneManager.Instance.UnlockCharacter(id);
            Unlock();
        }

        public void AssignItem(int id_, Sprite sprite, Character _character, bool isLock, ParticleSystem _rainbowFx)
        {
            id = id_;
            character = _character;
            character.transform.localPosition = Vector3.zero;
            character.enabled = false;

            rainbowFx = _rainbowFx;
            rainbowFx.transform.SetParent(transform);
            rainbowFx.transform.localPosition = Vector3.zero;

            canDrag = true;
            startParent = transform.parent;
            startPos = transform.position;
            startScale = transform.localScale;

            image.sprite = sprite;
            image.SetNativeSize();


            if (isLock)
            {
                Lock();
            }
            else
            {
                Unlock();
            }
        }

        public void SetToScrollView()
        {
            enabled = true;

            transform.SetParent(startParent);
            transform.SetSiblingIndex(startSiblingIdx);

            canDrag = true;
        }
        public void SetToFloor(Transform floorTrans)
        {
            enabled = false;
            character.enabled = true;
            character.transform.SetParent(floorTrans);
            character.AssginToFloor();
        }

        private void OnUnlockItem()
        {
            EventDispatcher.Instance.Dispatch(
                new EventKey.InitAdsPanel { instanceID = GetInstanceID(), spriteItem = image.sprite, nameObj = character.name, curPanel = "CharacterBoard" });
            GUIManager.instance.OpenPanel(PanelType.Ads);
        }
        public void Unlock()
        {
            canDrag = true;
            image.color = Color.white;
            image.DOFade(1, 0);
            lockBtn.transform.parent.gameObject.SetActive(false);
            lockBtn.gameObject.SetActive(false);
            rainbowFx.Play();
            RegisterEvent();
        }
        public void Lock()
        {
            canDrag = false;
            image.color = Color.black;
            image.DOFade(0.6f, 0);
            lockBtn.transform.parent.gameObject.SetActive(true);
            lockBtn.gameObject.SetActive(true);
            RemoveAllEvent();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            transform.SetParent(transform.parent.parent.parent);
            startPos = transform.position;
            //  image.gameObject.SetActive(false);
            //  character.gameObject.SetActive(true);

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragItem { characterScroll = this });
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            GameManager.instance.GetCurrentPosition(transform);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            canDrag = false;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragItem { characterScroll = this });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (punchScaleTween != null)
            {
                punchScaleTween?.Kill();
            }

            transform.localScale = startScale;
            punchScaleTween = transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.5f, 1);
        }
    }
}