using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace _WolfooShoppingMall
{
    public class NewCharacterWolfooScrollItem : MonoBehaviour
    {
        [SerializeField] Image iconImg;
        [SerializeField] Transform myHolder;
        [SerializeField] Button lockBtn;

        private EventTrigger trigger;
        private int startSiblingIdx;
        private Transform startParent;
        private PlayerScrollView myPanel;
        private CharacterWolfooWorld wolfoo;
        private int id;

        private void RemoveAllEventTrigger()
        {
            if (trigger != null)
                trigger.triggers.RemoveRange(0, trigger.triggers.Count);
        }
        private void RegisterMyEventTrigger()
        {
            trigger = gameObject.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(OnWatchAds);
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(OnWatchAds);
        }

        private void Start()
        {
            Setup();
            lockBtn.onClick.AddListener(OnClickLock);
        }

        private void OnDestroy()
        {
            if (trigger != null && trigger.triggers.Count > 0) RemoveAllEventTrigger();
        }

        private void OnWatchAds(EventKey.OnWatchAds obj)
        {
            if(obj.instanceID == GetInstanceID())
            {
                DataSceneManager.Instance.UnlockCharacter(id);
                Unlock();
            }
        }

        public void Unlock()
        {
            iconImg.color = Color.white;
            iconImg.DOFade(1, 0);
            lockBtn.gameObject.SetActive(false);
            RegisterMyEventTrigger();
        }
        public void Lock()
        {
            iconImg.color = Color.black;
            iconImg.DOFade(0.7f, 0);
            lockBtn.gameObject.SetActive(true);
            RemoveAllEventTrigger();
        }

        private void OnClickLock()
        {
            EventDispatcher.Instance.Dispatch(
                new EventKey.InitAdsPanel() { 
                    spriteItem = iconImg.sprite, 
                    instanceID = GetInstanceID(), 
                    curPanel = "PlayerScroll", 
                    nameObj = iconImg.sprite.name 
                });
            GUIManager.instance.OpenPanel(PanelType.Ads);
        }
        public void Assign(int id, bool isLock)
        {
            this.id = id;
            if (isLock) Lock();
            else Unlock();
        }

        public void Assign(PlayerScrollView panel, CharacterWolfooWorld wolfooPb, Sprite iconSprite)
        {
            myPanel = panel;
            if (wolfooPb != null)
            {
                wolfoo = Instantiate(wolfooPb, transform);
                wolfoo.transform.localScale *= 100;
                wolfoo.Setup();
                wolfoo.Hide(transform);
                wolfoo.GetEndDrag = GetCharacterEndDrag;
            }
            iconImg.sprite = iconSprite;
        }
        private void GetCharacterEndDrag(CharacterWolfooWorld wolfoo)
        {
            if (!myPanel.IsOpenPanel) return;

            var isInside = myPanel.CheckInSideScrollView(wolfoo);
            if(isInside)
            {
                SetToScrollView();
                myPanel.ScrollTo(transform);
            }
            else
            {
                SetToMap();
            }
            EventRoomBase.OnEndDragScrollCharacter?.Invoke(wolfoo, isInside);
        }

        public void Setup()
        {
            startSiblingIdx = transform.GetSiblingIndex();
            startParent = transform.parent;
        }

        private void OnPointerDown(PointerEventData data)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
            EventRoomBase.OnBeginDragScrollCharacter?.Invoke(wolfoo);

        }
        private void OnDrag(PointerEventData data)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
        private void OnPointerUp(PointerEventData data)
        {
            if (myPanel.IsOpenPanel)
            {
                var isInside = myPanel.CheckInSideScrollView(this);
                if (isInside)
                {
                    SetToScrollView();
                }
                else
                {
                    SetToMap();
                }
                EventRoomBase.OnEndDragScrollCharacter?.Invoke(wolfoo, isInside);
            }
            transform.localPosition = Vector3.zero;
        }

        private void SetToScrollView()
        {
            wolfoo.Hide(transform);
            myHolder.gameObject.SetActive(true);
            transform.SetParent(myHolder);
        }
        private void SetToMap()
        {
            wolfoo.Show();
            myHolder.gameObject.SetActive(false);
        }

        public void TurnBackScroll(int idx = -1)
        {
            transform.SetParent(startParent);
            transform.SetSiblingIndex(idx > 0 ? idx : startSiblingIdx);
            transform.localPosition = Vector3.zero;
        }
    }
}
