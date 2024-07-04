using _Base;
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
    public class Floor : MonoBehaviour
    {
        [SerializeField] Transform contentTrans;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform backTrans;
        [SerializeField] Transform groundTrans;
        [SerializeField] List<Transform> scrollLimiteds;
        [SerializeField] float velocity;
        [SerializeField] OrderCompare[] compareItems;
        [SerializeField] PlayerPanel playerPanelPb;
        [SerializeField] Food[] foods;
        [SerializeField] IngameType ingameSoundType;
        [SerializeField] bool canChangeFloor = true;
        [SerializeField] protected PanelType panelType;
        [SerializeField] bool isShowBanner;

        CityType MapController;
        private Tween delayTween;
        private float distanceLeft;
        private float distanceRight;
        protected PlayerPanel playerPanel;
        private Transform touchObj;
        private Vector3 beginTouch;
        private Vector3 touchPoint;
        private bool isMoveingFloor;

        private float changeFloorDistance = 4;

        public ScrollRect ScrollRect { get => scrollRect; }
        public PanelType PanelType { get => panelType; }

        #region INPUT METHOD
        public void OnChangeFloor(int index = -1)
        {
            if (index == -1)
            {
                GUIManager.instance.OpenPanel(PanelType.Elevator);
            }
        }

        public void Register(CityType cityType)
        {
            MapController = cityType;
            SetupScreen();
        }

        public void Spawn(GameObject mapPb)
        {
            Instantiate(mapPb, GUIManager.instance.canvasSpawnMode.transform);
        }
        #endregion

        protected virtual void OnDestroy()
        {
            if (delayTween != null) delayTween?.Kill();

            EventDispatcher.Instance.Dispatch(new _WolfooCity.EventKey.OnDestroyScene());
        }

        private void Awake()
        {
            playerPanel = Instantiate(playerPanelPb, transform);
            playerPanel.AssignBackFloor(PanelType.Intro, panelType);

            scrollRect.content.gameObject.SetActive(false);

            playerPanel.AssignCity(MapController);
        }
        private void Update()
        {
            scrollRect.enabled = GameManager.instance.CanMoveFloor;
        }

        protected virtual void Start()
        {
            scrollRect.content.gameObject.SetActive(true);

            if (!BaseDataManager.Instance.playerMe.IsCityShowed(MapController))
            {
                var v = 1500f;
                var s = scrollRect.content.sizeDelta.x;
                var t = s / v;
                UISetupManager.Instance.maskBg.gameObject.SetActive(true);
                scrollRect.horizontalScrollbar.value = 0;
                scrollRect.DOHorizontalNormalizedPos(1, t).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    scrollRect.DOHorizontalNormalizedPos(.4f, t/5).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        UISetupManager.Instance.maskBg.gameObject.SetActive(false);
                        playerPanel.OnShowBoard();
                    });
                });
            }
            else
            {
                playerPanel.OnShowBoard();
            }

            backTrans.anchoredPosition = transform.position;
            backTrans.anchorMin = new Vector2(0, 0);
            backTrans.anchorMax = new Vector2(1, 1);
            backTrans.pivot = new Vector2(0.5f, 0.5f);
            //  backTrans.sizeDelta = transform.GetComponent<RectTransform>().rect.size;
            backTrans.sizeDelta = Vector3.zero;
            backTrans.position = Vector3.zero;

            for (int i = 0; i < compareItems.Length; i++)
            {
                foreach (BackItem backItem in compareItems[i].items)
                {
                    backItem.InitOrder(i);
                }
            }
            foreach (var food in foods)
            {
                food.AssignDrag();
            }

        }
        protected virtual void OnEnable()
        {
            if (canChangeFloor)
            {
                scrollRect.GetComponent<SlideObj>().GetDrag += OnDrag;
                scrollRect.GetComponent<SlideObj>().GetBeginDrag += OnBeginDrag;
                scrollRect.GetComponent<SlideObj>().GetEndDrag += OnEndDrag;
            }

            EventDispatcher.Instance.RegisterListener<EventKey.OnDragBackItem>(GetDragBackItem);
            delayTween = DOVirtual.DelayedCall(0.2f, () =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem
                {
                    scrollRect = scrollRect,
                    ground = groundTrans,
                });

                if (SoundManager.instance != null)
                    SoundManager.instance.PlayIngame(ingameSoundType);
            });
            if (isShowBanner) AdsManager.Instance.ShowBanner();
        }

        protected virtual void OnDisable()
        {
            if (delayTween != null) delayTween?.Kill();
            EventDispatcher.Instance.RemoveListener<EventKey.OnDragBackItem>(GetDragBackItem);

            if (canChangeFloor)
            {
                scrollRect.GetComponent<SlideObj>().GetDrag -= OnDrag;
                scrollRect.GetComponent<SlideObj>().GetBeginDrag -= OnBeginDrag;
                scrollRect.GetComponent<SlideObj>().GetEndDrag -= OnEndDrag;
            }
            if (isShowBanner) AdsManager.Instance.HideBanner();
        }

        void SetupScreen()
        {
            if (Camera.main.aspect >= 1.7)
            {
                //   Debug.Log("16:9");
                switch (MapController)
                {
                    case CityType.Hospital:
                        contentTrans.localScale = Vector3.one * 1.15f;
                        break;
                    case CityType.House:
                        contentTrans.localScale = Vector3.one * 1;
                        break;
                    case CityType.Mall:
                        contentTrans.localScale = Vector3.one * 0.6f;
                        break;
                    case CityType.Playground:
                        contentTrans.localScale = Vector3.one;
                        break;
                    case CityType.Opera:
                        contentTrans.localScale = Vector3.one * 1.21f;
                        break;
                }
            }
            else if (Camera.main.aspect >= 1.5)
            {
                //     Debug.Log("3:2");
                switch (MapController)
                {
                    case CityType.Hospital:
                        contentTrans.localScale = Vector3.one * 1.15f;
                        break;
                    case CityType.House:
                        contentTrans.localScale = Vector3.one * 1;
                        break;
                    case CityType.Mall:
                        contentTrans.localScale = Vector3.one * 0.6f;
                        break;
                    case CityType.Playground:
                        contentTrans.localScale = Vector3.one;
                        break;
                    case CityType.Opera:
                        contentTrans.localScale = Vector3.one * 1.21f;
                        break;
                }
            }
            else
            {
                //    Debug.Log("4:3");
                switch (MapController)
                {
                    case CityType.Hospital:
                        contentTrans.localScale = Vector3.one * 1.53f;
                        break;
                    case CityType.House:
                        contentTrans.localScale = Vector3.one * 1.32f;
                        break;
                    case CityType.Mall:
                        contentTrans.localScale = Vector3.one * 0.8f;
                        break;
                    case CityType.Playground:
                        contentTrans.localScale = Vector3.one * 1.35f;
                        break;
                    case CityType.Opera:
                        contentTrans.localScale = Vector3.one * 1.55f;
                        break;
                }
            }
        }

        private void GetDragBackItem(EventKey.OnDragBackItem obj)
        {
            if (obj.backItem == null) return;
            if (!GameManager.instance.CanMoveFloor) return;
            //if (curBackItem == null)
            //    curBackItem = obj.backItem;
            //isDragging = true;
            distanceLeft = obj.backItem.transform.position.x - scrollLimiteds[0].position.x;
            distanceRight = scrollLimiteds[1].position.x - obj.backItem.transform.position.x;

            if (distanceLeft < 1f)
            {
                if (scrollRect.horizontalScrollbar.value == 0) return;
                scrollRect.horizontalScrollbar.value -= velocity;
            }

            if (distanceRight < 1f)
            {
                if (scrollRect.horizontalScrollbar.value == 1) return;
                scrollRect.horizontalScrollbar.value += velocity;
            }
        }

        void OnBeginDrag(PointerEventData eventData)
        {
            if (!canChangeFloor || playerPanel.IsOpen) return;
            if (!GameManager.instance.CanMoveFloor) return;
            if (touchObj == null)
            {
                touchObj = Instantiate(UISetupManager.Instance.center, transform.parent);
            }
            GameManager.instance.GetCurrentPosition(touchObj);
            beginTouch = touchObj.position;
            touchPoint = touchObj.position;
        }
        void OnEndDrag(PointerEventData eventData)
        {
            if (!canChangeFloor || playerPanel.IsOpen) return;
            if (!GameManager.instance.CanMoveFloor) return;
            isMoveingFloor = false;
            if (Mathf.Abs(transform.position.y) >= changeFloorDistance)
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { floor = this, direction = touchObj.position.y > beginTouch.y ? Direction.Down : Direction.Up });
            }
            transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canChangeFloor || playerPanel.IsOpen) return;
            if (!GameManager.instance.CanMoveFloor) return;

            GameManager.instance.GetCurrentPosition(touchObj);

            if (isMoveingFloor)
            {
                OnMove(0.1f);
            }
            else
            {
                if (Mathf.Abs(touchObj.position.y - touchPoint.y) >= 1.5f)
                {
                    OnMove(1.5f);
                    isMoveingFloor = true;
                }
            }
        }

        void OnMove(float velocity)
        {
            if (touchObj.position.y - touchPoint.y > 0f)
            {
                transform.position += new Vector3(0, 1, 0) * 0.0001f;
                if (GameManager.instance.CurFloorIdx == 0)
                {
                    if (transform.position.y > 0)
                    {
                        touchPoint = touchObj.position;
                        return;
                    }
                }
                transform.position += new Vector3(0, 1, 0) * velocity;
            }
            if (touchObj.position.y - touchPoint.y < 0f)
            {
                transform.position += new Vector3(0, -1, 0) * 0.0001f;
                if (GameManager.instance.CurFloorIdx == 2)
                {
                    if (transform.position.y < 0)
                    {
                        touchPoint = touchObj.position;
                        return;
                    }
                }
                transform.position += new Vector3(0, -1, 0) * velocity;
            }
            touchPoint = touchObj.position;
        }
    }

    [System.Serializable]
    public struct OrderCompare
    {
        public BackItem[] items;
    }
}