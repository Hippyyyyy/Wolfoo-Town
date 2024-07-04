using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.UIManager;

namespace _WolfooShoppingMall
{
    public class FloorWorld : MonoBehaviour
    {
        [System.Serializable]
        public struct ScreenDragLimit
        {
            /// <summary>
            /// Normal Screen
            /// </summary>
            public Vector3 normalSize;
            /// <summary>
            /// Wide Screen
            /// </summary>
            public Vector3 wideSize;
            /// <summary>
            /// 21 : 9
            /// </summary>
            public Vector3 longSize;
            /// <summary>
            /// Too Fucking Long
            /// </summary>
            public Vector3 longSize3;
        }

        [SerializeField] Transform scrollContent;
        [NaughtyAttributes.OnValueChanged("OnChangeLeftScrollBG")]
        [SerializeField] ScreenDragLimit screenDragLimitLeft;
        [NaughtyAttributes.OnValueChanged("OnChangeRightScrollBG")]
        [SerializeField] ScreenDragLimit screenDragLimitRight;
        [SerializeField] RoomFloorConfig config;

        private UIManager myUi;
        private bool isDragging;
        private float speed;
        private Vector3 velocity;
        private float dragMaxDistance;
        private float dragMinDistance;
        private Vector3 beginTouch;
        private Vector3 touchPos;
        private bool isAssigned;
        private bool canDrag;
        private float dragLimitLeftPos;
        private float dragLimitRightPos;
        private Tweener _tweenMove;

        private void Start()
        {

            EventRoomBase.OnDragBackItem += GetDragBackItem;
            BeachVillaEventManager.OnDragCustomItem += GetDragScrollItem;
            BeachVillaEventManager.OnBeginDragCustomItem += GetBeginDragScrollItem;
            BeachVillaEventManager.OnEndDragCustomItem += GetEndDragScrollItem;
        }

        private void OnDestroy()
        {
            EventRoomBase.OnDragBackItem -= GetDragBackItem;
            BeachVillaEventManager.OnDragCustomItem -= GetDragScrollItem;
            BeachVillaEventManager.OnBeginDragCustomItem -= GetBeginDragScrollItem;
            BeachVillaEventManager.OnEndDragCustomItem -= GetEndDragScrollItem;
        }
#if UNITY_EDITOR
        public int idxTestScroll;
        private void OnChangeLeftScrollBG()
        {
            switch (idxTestScroll)
            {
                case 1:
                    scrollContent.transform.localPosition = screenDragLimitLeft.normalSize;
                    break;
                case 2:
                    scrollContent.transform.localPosition = screenDragLimitLeft.wideSize;
                    break;
                case 3:
                    scrollContent.transform.localPosition = screenDragLimitLeft.longSize;
                    break;
                case 4:
                    scrollContent.transform.localPosition = screenDragLimitLeft.longSize3;
                    break;
            }
        }
        private void OnChangeRightScrollBG()
        {
            switch (idxTestScroll)
            {
                case 1:
                    scrollContent.transform.localPosition = screenDragLimitRight.normalSize;
                    break;
                case 2:
                    scrollContent.transform.localPosition = screenDragLimitRight.wideSize;
                    break;
                case 3:
                    scrollContent.transform.localPosition = screenDragLimitRight.longSize;
                    break;
                case 4:
                    scrollContent.transform.localPosition = screenDragLimitRight.longSize3;
                    break;
            }
        }
#endif

        private void AssignLimitPos()
        {
            if (Camera.main.aspect >= 2.5f)
            {
                //     Debug.Log("23:9");
                dragLimitLeftPos = screenDragLimitLeft.longSize3.x;
                dragLimitRightPos = screenDragLimitRight.longSize3.x;
            }
            else if (Camera.main.aspect >= 16f/9f)
            {
                // 21:9  Debug.Log("19:9"); // Long Size // 2400 x 1080
                dragLimitLeftPos = screenDragLimitLeft.longSize.x;
                dragLimitRightPos = screenDragLimitRight.longSize.x;
            }
            else if (Camera.main.aspect >= 1.5)
            {
                //     Debug.Log("3:2");
                dragLimitLeftPos = screenDragLimitLeft.normalSize.x;
                dragLimitRightPos = screenDragLimitRight.normalSize.x;
            }
            else
            {
                //    Debug.Log("4:3");
                dragLimitLeftPos = screenDragLimitLeft.wideSize.x;
                dragLimitRightPos = screenDragLimitRight.wideSize.x;
            }

            //dragLimitLeftPos = screenDragLimitLeft.position.x;
            //dragLimitRightPos = screenDragLimitRight.position.x;
        }

        public void PreviewMap(System.Action OnCompleted = null)
        {
            canDrag = false;
            _tweenMove = scrollContent.DOLocalMoveX(dragLimitLeftPos, 10).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                _tweenMove = scrollContent.DOLocalMoveX(dragLimitRightPos, 20).SetSpeedBased(true).OnComplete(() =>
                {
                    canDrag = true;
                    OnCompleted?.Invoke();
                });
            });
        }

        private void GetEndDragScrollItem(CustomRoomItem obj)
        {
            Assign(true);
        }

        private void GetBeginDragScrollItem(CustomRoomItem obj)
        {
            Assign(false);
        }
        private void GetDragScrollItem(CustomRoomItem obj)
        {
            CompareItemToScroll(obj.transform);
        }

        public void Assign(bool canDrag)
        {
            this.canDrag = canDrag;
        }
        public void Assign()
        {
            speed = config.speed;
#if UNITY_EDITOR
            speed *= 2;
#endif
            velocity = Vector3.right * speed;
            dragMaxDistance = config.dragDetectMax; 
            dragMinDistance = config.dragDetectMin;
            myUi = GameManager.instance.UiManager;
            AssignLimitPos();
            scrollContent.localPosition = new Vector3(dragLimitRightPos, scrollContent.position.y, 0);
        }

        private void GetDragBackItem(BackItemWorld obj)
        {
            if (Mathf.Abs(obj.transform.position.x - myUi.limitArea.leftLimit.position.x) < 1)
            {
                var movePos = Vector3.right * speed/3;
                ContentMove(movePos);
            }
            else if (Mathf.Abs(obj.transform.position.x - myUi.limitArea.rightLimit.position.x) < 1)
            {
                var movePos = Vector3.left * speed/3;
                ContentMove(movePos);
            }
        }

        void CompareItemToScroll(Transform obj)
        {
            if (Mathf.Abs(obj.position.x - myUi.limitArea.leftLimit.position.x) < 1)
            {
                var movePos = Vector3.right * speed;
                ContentMove(movePos);
            }
            else if (Mathf.Abs(obj.position.x - myUi.limitArea.rightLimit.position.x) < 1)
            {
                var movePos = Vector3.left * speed;
                ContentMove(movePos);
            }
        }

        private void OnBeginDrag()
        {
            beginTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            EventRoomBase.OnBeginDragFloor?.Invoke();
        }
        private void OnDrag()
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var movePos = Vector3.zero;
            if (touchPos.x - beginTouch.x > dragMaxDistance)
            {
                movePos = Vector3.right * speed;
                beginTouch = touchPos;
            }
            else if (beginTouch.x - touchPos.x > dragMaxDistance)
            {
                movePos = Vector3.left * speed;
                beginTouch = touchPos;
            }
            ContentMove(movePos);
        }

        private void ContentMove(Vector3 movePos)
        {
            scrollContent.position = Vector3.SmoothDamp(scrollContent.position, scrollContent.position + movePos, ref velocity, config.smoothTime);
            if (scrollContent.localPosition.x <= dragLimitLeftPos)
            {
                scrollContent.localPosition = new Vector3(dragLimitLeftPos, scrollContent.localPosition.y, 0);
            }
            if (scrollContent.localPosition.x >= dragLimitRightPos)
            {
                scrollContent.localPosition = new Vector3(dragLimitRightPos, scrollContent.localPosition.y, 0);
            }
        }
        private void OnEndDrag()
        {
            EventRoomBase.OnEndDragFloor?.Invoke();

        }

        private void OnMouseDown()
        {
            if (!canDrag) return;
            OnBeginDrag();
        }
        private void OnMouseDrag()
        {
            if (!canDrag) return;
            OnDrag();
        }
        private void OnMouseUp()
        {
            if (!canDrag) return;
            OnEndDrag();
        }
    }
}