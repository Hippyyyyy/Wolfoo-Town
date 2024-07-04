using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class BeverageLid : BackItem
    {
        private void OnDestroy()
        {
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { beverageLid = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { beverageLid = this, backitem = this });
        }

        protected override void InitItem()
        {
            isScaleDown = true;
            canDrag = true;
            scaleIndex = 0.5f;
            IsCarry = true;
            isComparePos = true;
        }
        public void OnPlugin(Transform _endParent, System.Action OnComplete)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(Vector3.zero, 50, 1, 0.5f)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
        }
    }
}