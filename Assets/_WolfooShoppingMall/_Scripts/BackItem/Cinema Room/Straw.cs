using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Straw : BackItem
    {
        private Tweener rotateTween;
        private Quaternion startRotate;

        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            scaleIndex = 0.5f;
            IsCarry = true;
        }
        protected override void Start()
        {
            base.Start();
            startRotate = transform.rotation;
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(startRotate.eulerAngles, 0.25f);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, straw = this });
        }

        public void OnPlugin(Transform _endParent)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_endParent);
            rotateTween = transform.DORotate(Vector3.zero, 0.5f);
            tweenJump = transform.DOLocalJump(Vector3.zero, 50, 1, 0.5f)
            .OnComplete(() =>
            {
            });
        }
        public void OnJumpToPlastic(Transform _endParent)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(Vector3.zero + Vector3.up * 110, 50, 1, 0.5f);
        }
    }
}