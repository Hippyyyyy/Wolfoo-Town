using _Base;
using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class Room3 : Panel
    {
        [SerializeField] Transform groundTrans;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] PlayerPanel mainPanelPb;
        [SerializeField] Image coverImg;
        [SerializeField] List<RectTransform> anchors;
        [SerializeField] float velocity = 0.01f;
        [SerializeField] Transform flowerZone;
        [SerializeField] Button clayModeBtn;
        [SerializeField] PanelType nextMode;
        [SerializeField] PanelType panelType;
        [SerializeField] bool isScaleMap;

        // [SerializeField] string tagName;
        [SerializeField] Button backBtn;
        private float distanceLeft;
        private float distanceRight;
        private int nextFlowerIdx;
        private Tween delayTween;

        public Transform GroundTrans { get => groundTrans; }
        public ScrollRect ScrollRect { get => scrollRect; }

        protected override void Awake()
        {
            var panel = Instantiate(mainPanelPb, transform);
            panel.AssignPanel(PanelType.Room3);

            if(isScaleMap)
            {
                _WolfooShoppingMall.GameManager.GetScreenRatio(() =>
                {
                    scrollRect.content.localScale = Vector3.one * 0.5f;
                }, () =>
                {
                    scrollRect.content.localScale = Vector3.one * 0.5f;
                }, () =>
                {
                    scrollRect.content.localScale = Vector3.one * 0.64f;
                });
            }
        }
        protected override void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnModeComplete>(GetModeComplete);
            backBtn.onClick.AddListener(OnBack);
            clayModeBtn.onClick.AddListener(OnPlayMode);

            clayModeBtn.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 4).SetLoops(-1);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                GUIManager.instance.room3 = this;
                EventManager.GetMainPanel?.Invoke(scrollRect.content.gameObject, groundTrans.gameObject);
            });

            coverImg.gameObject.SetActive(true);
            if (!BaseDataManager.Instance.playerMe.IsCityShowed(CityType.School))
            {
                var v = 1300f;
                var s = scrollRect.content.sizeDelta.x;
                var t = s / v;
                coverImg.gameObject.SetActive(true);
                scrollRect.horizontalScrollbar.value = 0;
                scrollRect.DOHorizontalNormalizedPos(1, t)
                .OnComplete(() =>
                {
                    scrollRect.DOHorizontalNormalizedPos(.4f, t/3)
                    .OnComplete(() =>
                    {
                        coverImg.gameObject.SetActive(false);
                    });
                });
            }
            else
            {
                coverImg.gameObject.SetActive(false);
            }

            scrollRect.content.gameObject.SetActive(false);
            scrollRect.content.gameObject.SetActive(true);

            if (GameManager.instance.isLoadCompleted) EventDispatcher.Instance.Dispatch(new EventKey.OnLoadDataCompleted());
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnModeComplete>(GetModeComplete);
        }

        private void OnEnable()
        {
            EventManager.OnDragBackItem += GetDragBackItem;
            if (delayTween != null) delayTween?.Kill();
            delayTween = DOVirtual.DelayedCall(0.2f, () =>
            {
                GUIManager.instance.AssignPanel(scrollRect.content.gameObject, groundTrans.gameObject);
            });
        }
        private void OnDisable()
        {
            EventManager.OnDragBackItem -= GetDragBackItem;
        }

        private void OnPlayMode()
        {
            EventManager.OnPlaygame?.Invoke(this, nextMode);
        }

        private void GetModeComplete(EventKey.OnModeComplete item)
        {
            if (item.flowerPot != null)
            {
                if (nextFlowerIdx >= flowerZone.childCount)
                {
                    nextFlowerIdx = 0;
                }
                if (flowerZone.GetChild(nextFlowerIdx).childCount > 0)
                    flowerZone.GetChild(nextFlowerIdx).GetChild(0).gameObject.SetActive(false);
                item.flowerPot.transform.SetParent(flowerZone.GetChild(nextFlowerIdx));
                item.flowerPot.transform.localPosition = Vector3.zero;
                item.flowerPot.transform.SetAsFirstSibling();
                item.flowerPot.transform.localScale = Vector3.one * 0.7f;
                item.flowerPot.enabled = false;

                if(item.flowerPot.GetComponent<RoomFlower>() == null)
                {
                    item.flowerPot.gameObject.AddComponent<RoomFlower>().Init();
                }

                GUIManager.instance.rainbowFx.transform.position = item.flowerPot.transform.position;
                GUIManager.instance.rainbowFx.Play();
                SoundManager.instance.PlayWolfooSfx(SfxWolfooType.Hoow);

                Invoke("StopRainbow", 2);

                nextFlowerIdx++;
            }
        }
        private void StopRainbow()
        {
            GUIManager.instance.rainbowFx.Stop();
        }

        private void GetDragBackItem(Transform curTrans)
        {
            distanceLeft = curTrans.position.x - anchors[0].position.x;
            distanceRight = anchors[1].position.x - curTrans.position.x;

            if (distanceLeft < 2)
            {
                if (scrollRect.horizontalScrollbar.value == 0) return;
                scrollRect.horizontalScrollbar.value -= velocity;
            }

            if (distanceRight < 2)
            {
                if (scrollRect.horizontalScrollbar.value == 1) return;
                scrollRect.horizontalScrollbar.value += velocity;
            }
        }
        private void OnUpdateScroll(float value)
        {
            scrollRect.horizontalScrollbar.value = value;
        }

        private void OnBack()
        {
            EventManager.OnBackPanel?.Invoke(this, PanelType.Main, false);
        }
    }
}