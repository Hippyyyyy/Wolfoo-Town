using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Ball : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, ball = this });
        }

        public void JumpToBasket(Vector3 _endPos, Vector3 _endPos2, Transform _endParent, System.Action OnJumpInto, System.Action OnComplete)
        {
            if (!canDrag) return;
            canDrag = false;

            canMoveToGround = false;
            transform.SetParent(_endParent);
            if (tweenMove != null) tweenMove?.Kill();
            transform.DOLocalJump(_endPos, 500, 1, 0.5f).OnComplete(() =>
            {
                OnJumpInto?.Invoke();
                transform.DOLocalMove(_endPos2, 0.26f).OnComplete(() =>
                {
                    canMoveToGround = true;
                    MoveToGround();
                    transform.localScale = startScale;
                    canDrag = true;
                });
            });
        }
    }
}