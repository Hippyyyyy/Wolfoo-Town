using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using _Base;

namespace _WolfooSchool
{
    public class MainPanel : Panel
    {
        [SerializeField] Transform groundTrans;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] PlayerPanel mainPanelPb;
        [SerializeField] Image coverImg;
        [SerializeField] Button backBtn;
        [SerializeField] List<RectTransform> anchors;
        [SerializeField] float velocity;
        [SerializeField] PanelType panelType;

        private float distanceLeft;
        private float distanceRight;
        private Tween delayTween;
        private Tween _tween;

        public Transform GroundTrans { get => groundTrans; }
        public ScrollRect ScrollRect { get => scrollRect; }

        protected override void Awake()
        {
            var panel = Instantiate(mainPanelPb, transform);
            panel.AssignPanel(PanelType.Main);
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }
        protected override void Start()
        {
            base.Start();
            backBtn.onClick.AddListener(OnBack);

            _tween =  DOVirtual.DelayedCall(0.1f, () =>
            {
                GUIManager.instance.mainPanel = this;
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
        private void OnBack()
        {
        //    LoadSceneManager.Instance.OnLoadScene("WolfooCity_Scene");
            EventManager.OnBackPanel?.Invoke(this, PanelType.Intro, false);
        }

        void LoadRoom()
        {
        }
    }
}