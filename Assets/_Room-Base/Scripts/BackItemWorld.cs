using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _WolfooShoppingMall
{
    public class BackItemWorld : ItemWorldBase
    {
        [SerializeField] SpriteRenderer mySprite;
        [SerializeField] AudioClip myAudioClip;

        private SpriteRenderer[] mySprites;
        private RoomFloorConfig roomConfig;
        private SortingGroup sortingGroup;

        public Action<int> OnChangeSorting;

        protected bool IsComparePos;
        protected bool IsScaleDown;
        protected bool IsAssigned;
        protected bool isDragging;

        protected UIManager _myMap;
        private bool canClick;
        private bool canDrag;
        private Vector3 downPos;
        private Tween _tweenDelayDrag;
        private Sequence _tweenJump;

        public enum SwearItemType
        {
            None,
            Pants,
            Eyes,
            Mouth,
            Head,
            Hands
        }

        private bool isCharacter;
        private bool isBlockMultiDrag;
        public TableWorld triggleTable;
        public CharacterWorld triggerCharacter;
        private bool canStandOnTable;
        private Vector3 startScale;
        private Tweener _tweenMove;
        private bool isRegisterTrigger;
        private bool isTouched;

        protected Sequence tweenDance { get; private set; }

        public int Id { get; private set; }
        public bool IsCarryItem { get; protected set; }
        public bool IsFood { get; protected set; }
        public bool IsBeverage { get; protected set; }
        public bool IsDragable { get; protected set; }
        public bool IsClick { get; protected set; }
        public bool IsStandingOnTable { get; protected set; }
        public bool IsCharacter { get => isCharacter; protected set => isCharacter = value; }
        public bool IsSwearItem { get { return SwearType != SwearItemType.None; } }
        public SwearItemType SwearType { get; protected set; }
        public SpriteRenderer MySprite { get => mySprite; }
        public int LayerOrder
        {
            get
            {
                if (sortingGroup != null) return sortingGroup.sortingOrder;
                else if (MySprite != null) return MySprite.sortingOrder;
                else return 0;
            }
            set
            {
                if (sortingGroup != null) sortingGroup.sortingOrder = value;
                else if (MySprite != null) MySprite.sortingOrder = value;
                var pos = new Vector3(transform.localPosition.x, transform.localPosition.y, value * -0.001f - 1f);
                if (pos.z > 0) pos.z = -0.001f;
                transform.localPosition = pos;

                OnChangeSorting?.Invoke(value);
            }
        }

        private void OnEnable()
        {
            RegisterEvent();
        }
        private void OnDisable()
        {
            RemoveEvent();
        }
        protected override void OnKill()
        {
            if (_tweenDelayDrag != null) _tweenDelayDrag?.Kill();
            KillDragging();
            KillAllScaling();
        }

        public void SetToBackground()
        {
            transform.SetParent(_myMap.ItemContent);
        }

        public override void Setup()
        {
            if (IsAssigned) return;
            IsAssigned = true;

            EventRoomBase.OnInitBackItem?.Invoke(this);
            Id = GameManager.instance.GenerateBackItemId(this);

            canClick = IsClick;
            canDrag = IsDragable;
            canStandOnTable = IsStandingOnTable;
            startScale = transform.localScale;

            sortingGroup = GetComponent<SortingGroup>();
            mySprites = transform.GetComponentsInChildren<SpriteRenderer>();
            roomConfig = GameManager.instance.RoomConfig;

            if (IsDragable)
            {
                SetupDragItem();
            }
         
        }
        private void SetupDragItem()
        {
            OrderLayerIndex();
            var myBox = GetComponentInChildren<Collider2D>();
            myBox.isTrigger = true;
        }
        protected override void RegisterEvent()
        {
            EventRoomBase.OnEndDragBackItem += GetEndDragBackItem;
            EventRoomBase.OnBeginDragBackItem += GetBeginDragBackItem;
        }

        protected override void RemoveEvent()
        {
            EventRoomBase.OnEndDragBackItem -= GetEndDragBackItem;
            EventRoomBase.OnBeginDragBackItem -= GetBeginDragBackItem;
        }

        public void AssignTempDrag(bool tempCanDrag, bool isFirstActive)
        {
            canDrag = IsDragable && tempCanDrag;

            if (isFirstActive)
            {
                _myMap = GameManager.instance.UiManager;
                sortingGroup = GetComponent<SortingGroup>();
                roomConfig = GameManager.instance.RoomConfig;
            }
        }
        private void TouchedThisObject()
        {
            if (!isTouched)
            {
                isTouched = true;
                EventDispatcher.Instance.Dispatch(new EventKey.OnTouchBackItem { backItemWorld = this });
            }
        }

        #region PUBLIC METHODS
        public void BeginDrag()
        {
            if (isBlockMultiDrag) return;
            isBlockMultiDrag = true;
            _tweenDelayDrag = DOVirtual.DelayedCall(0.5f, () => { isBlockMultiDrag = false; });

            if (GameManager.instance.BackItemIsDragging && GameManager.instance.CurrentDragBackItem != this) return;
            isDragging = true;

            OnBeginDrag();
        }

        internal void PlaySound()
        {
            if(myAudioClip != null)
            {

            }
        }

        public void Drag()
        {
            if (!isDragging) return;

            OnDrag();
        }
        public void EndDrag()
        {
            if (!isDragging) return;
            isDragging = false;

            OnEndDrag();
        }
        public void Setup(UIManager myMap)
        {
            _myMap = myMap;
        }
        public void StopTrigger()
        {
            StopMovingToGround();
            KillDragging();
        }
        public virtual void JumpTo(Vector3 _endPos, System.Action OnCompleted)
        {
            StopMovingToGround();
            KillDragging();
            _tweenJump = transform.DOJump(_endPos, 1, 1, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
            {
                OnCompleted?.Invoke();
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.FallingDown);
            });
        }
        public void JumpToTable(Transform endParent)
        {
            StopMovingToGround();
            transform.SetParent(endParent);
            if (_tweenMove != null) _tweenJump?.Kill();
            _tweenJump = transform.DOJump(transform.position, 0.1f, 1, 0.2f).OnComplete(() =>
            {
                LayerOrder = triggleTable.LayerOrder > 0 ? triggleTable.LayerOrder + 3 : triggleTable.LayerOrder - 1;
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.FallingDown);
            });
        }
        protected void PlayWithBox(bool isInside, string sortingLayer = "")
        {
        //    isRegisterTrigger = false;
            foreach (var item in mySprites)
            {
                if (isInside)
                {
                    item.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }
                else
                {
                    item.maskInteraction = SpriteMaskInteraction.None;
                }
            }
            if (!sortingLayer.Equals("")) sortingGroup.sortingLayerName = sortingLayer;
            if (isInside) StopMovingToGround();
            OrderLayerIndex();
        }
        protected virtual void OrderLayerIndex(bool isMax = false)
        {
            if (isMax)
            {
                LayerOrder = 1000;
            }
            else
            {
                if (transform.parent == GameManager.instance.UiManager.ItemContent)
                {
                    LayerOrder = (int)(transform.localPosition.y * -100);
                }
                else
                {
                    LayerOrder = (int)(transform.position.y * -100);
                }
            }
        }
        public virtual void KillDragging()
        {
            if (_tweenJump != null) _tweenJump?.Kill();
        }
        #endregion

        #region PROTECT METHODS
        protected virtual void ReleaseStatement(BackItemWorld backItem)
        {

        }
        protected virtual void ExcuteItemStatement()
        {
            if(triggerCharacter && IsCarryItem)
            {
                var orderElement = triggerCharacter.OrderElement;

                JumpTo(orderElement.element.transform.position, () =>
                {
                    transform.SetParent(orderElement.element.transform);
                    LayerOrder = orderElement.maxLayer;
                    PlaySound();
                });
                return;
            }

            if(triggleTable && canStandOnTable)
            {
                JumpToTable(triggleTable.transform);
                return;
            }
        }
        protected virtual void StopMovingToGround()
        {
            isRegisterTrigger = false;
            if (_tweenMove != null) _tweenMove?.Kill();
        }
        public virtual void PlayMovingToGround(bool isForce = false)
        {
            if (isForce)
            {
                isRegisterTrigger = true;
            }
            transform.SetParent(_myMap.ItemContent);
            if (_tweenMove != null) _tweenMove?.Kill();
            _tweenMove = transform.DOMoveY(-50, 1).SetSpeedBased(true).SetEase(Ease.OutBack);
        }
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (isRegisterTrigger)
            {
                Debug.Log($"Back Item {name} is TriggerStay with: {collision.gameObject.name}");
                if (roomConfig != null)
                {
                    if (collision.gameObject.layer == roomConfig.FLOOR_LAYER)
                    {
                        SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.FallingDown);
                        StopMovingToGround();
                        OrderLayerIndex();
                    }
                }
            }
        }
        protected virtual void OnClick() { 
            TouchedThisObject();
        }
        protected virtual void OnBeginDrag()
        {
            isRegisterTrigger = false;
            transform.SetParent(_myMap.ItemContent);
            StopMovingToGround();
            KillDragging();
            OrderLayerIndex(true);

            var mousePos = Input.mousePosition;
            var movepos = Camera.main.ScreenToWorldPoint(mousePos);
            movepos.z = 0;
            OnCheckLimitDragging();
            transform.position = movepos;

            TouchedThisObject();
            EventRoomBase.OnBeginDragBackItem?.Invoke(this);
        }
        protected virtual void OnDrag()
        {
            var mousePos = Input.mousePosition;
            var movepos = Camera.main.ScreenToWorldPoint(mousePos);
            movepos.z = 0;
            transform.position = movepos;
            OnCheckLimitDragging();

            EventRoomBase.OnDragBackItem?.Invoke(this);
        }
        protected void OnCheckLimitDragging()
        {
            if (transform.position.x <= _myMap.limitArea.leftLimit.position.x)
            {
                transform.position = new Vector3(_myMap.limitArea.leftLimit.position.x, transform.position.y, 0);
            }
            if (transform.position.x >= _myMap.limitArea.rightLimit.position.x)
            {
                transform.position = new Vector3(_myMap.limitArea.rightLimit.position.x, transform.position.y, 0);
            }
            if (transform.position.y >= _myMap.limitArea.upLimit.position.y)
            {
                transform.position = new Vector3(transform.position.x, _myMap.limitArea.upLimit.position.y, 0);
            }
            if (transform.position.y <= _myMap.limitArea.downLimit.position.y)
            {
                transform.position = new Vector3(transform.position.x, _myMap.limitArea.downLimit.position.y, 0);
            }
        }
        protected virtual void OnEndDrag()
        {
            isRegisterTrigger = true;
            PlayMovingToGround();
            ExcuteItemStatement();
            EventRoomBase.OnEndDragBackItem?.Invoke(this);
        }
        protected void KillAllScaling()
        {
            if (tweenDance != null)
                tweenDance?.Kill();
        }
        protected void KillScaling(Sequence sequence)
        {
            if (sequence != null)
                sequence?.Kill();
        }

        protected void Dance()
        {
            tweenDance?.Kill();
            transform.localScale = startScale;
            tweenDance = DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 2));
        }
        protected virtual void GetEndDragBackItem(BackItemWorld obj) { }
        protected virtual void GetBeginDragBackItem(BackItemWorld obj) { }
        #endregion

        #region PRIVATE METHODS
        private void OnMouseDown()
        {
            downPos = Input.mousePosition;

            if (canDrag)
            {
                BeginDrag();
            }
        }
        private void OnMouseDrag()
        {
            if (canDrag)
            {
                Drag();
            }
        }
        private void OnMouseUp()
        {
            if (downPos == Input.mousePosition)
            {
                if (canClick) { OnClick(); }
            }
            if (canDrag)
            {
                EndDrag();
            }
        }

        #endregion

#if UNITY_EDITOR
#endif
    }
}