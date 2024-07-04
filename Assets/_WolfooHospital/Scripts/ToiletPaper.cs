using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ToiletPaper : TransformBackItem
    {
        private Tweener _tween;

        protected override void InitData()
        {
            base.InitData();
            isComparePos = true;
        }

        public void Spawn()
        {
            transform.localScale = Vector3.zero;
            _tween?.Kill();
            _tween = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                AssignDrag();
            });
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, paper = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            isTransform = false;
            SetState();

        }
    }
}