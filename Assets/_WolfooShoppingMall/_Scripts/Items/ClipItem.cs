using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ClipItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image thumbnailImg;
        [SerializeField] Image chooseImg;
        [SerializeField] Image iconImg;
        [SerializeField] Button adsBtn;

        private int id;
        private int subId;
        private Vector3 startScale;
        private Vector3 startScale2;
        private Tweener scaleTween;
        private Tweener scaleTween2;
        private bool isLock;

        private void Awake()
        {
            startScale = transform.localScale;
            startScale2 = iconImg.transform.localScale;
            adsBtn.onClick.AddListener(OnOpenAds);
        }

        private void OnEnable()
        {
            scaleTween2 = iconImg.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 5).SetLoops(-1, LoopType.Yoyo);

            EventDispatcher.Instance.RegisterListener<EventKey.OnSelect>(GetSelect);
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
        }
        private void OnDisable()
        {
            if (scaleTween2 != null) scaleTween2?.Kill();
            iconImg.transform.localScale = startScale2;

            EventDispatcher.Instance.RemoveListener<EventKey.OnSelect>(GetSelect);
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);
        }

        private void GetWatchAds(EventKey.OnWatchAds obj)
        {
            if (obj.instanceID == GetInstanceID())
            {
                DataSceneManager.Instance.UnlockVideo(id, subId);
                EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { idx = id, subIdx = subId, clipItem = this });

                OnUnlock();
            }
        }

        private void OnOpenAds()
        {
            EventDispatcher.Instance.Dispatch(
                new EventKey.InitAdsPanel
                {
                    idxItem = id,
                    idxSubItem = subId,
                    clipItem = this,
                    instanceID = GetInstanceID(),
                    spriteItem = thumbnailImg.sprite,
                    nameObj = name,
                    curPanel = "cinema_movie"
                });
            GUIManager.instance.OpenPanel(PanelType.Ads);
        }

        private void GetSelect(EventKey.OnSelect obj)
        {
            if (obj.clipItem == null) return;
            chooseImg.gameObject.SetActive(obj.idx != id || obj.subIdx != subId);
        }

        public void AssignItem(int _id, int _subId, Sprite sprite, bool _isLock)
        {
            id = _id;
            subId = _subId;
            thumbnailImg.sprite = sprite;
            isLock = _isLock;

            if (isLock) OnLock();
            else OnUnlock();
        }
        public void OnUnlock()
        {
            isLock = false;
            adsBtn.gameObject.SetActive(false);
        }
        public void OnLock()
        {
            isLock = true;
            adsBtn.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isLock) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { idx = id, subIdx = subId, clipItem = this });

            if (scaleTween2 != null) scaleTween2?.Kill();
            iconImg.transform.localScale = startScale2;

            if (scaleTween != null) scaleTween?.Kill();
            transform.localScale = startScale;
            scaleTween = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 3).SetEase(Ease.OutBack);
        }
    }
}