using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class FaceMask : BackItem
    {
        [SerializeField] float scaleIdx = 3;
        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isComparePos = true;
            canMoveToGround = true;
            isScaleDown = true;
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { backitem = this, faceMask = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, faceMask = this });
        }
        public void Swear(Transform _parent, bool isWolfoo = true)
        {
            IsAssigned = true;
            KillDragging();
            KillScalling();
            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            if (isWolfoo)
                transform.localScale = Vector3.one * scaleIdx;
            else
                transform.localScale = Vector3.one * scaleIdx*3;
        }
        public void Spawn()
        {
            startScale = transform.localScale;
            transform.localScale = Vector3.zero;
            canDrag = false;
            tweenScale = transform.DOScale(startScale, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canDrag = true;
            });
        }
    }
}