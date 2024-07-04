using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class PizzaTopping : BackItem
    {
        private Tweener scaleTween;

        protected override void InitItem()
        {
            IsFood = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        private void OnDestroy()
        {
            if (scaleTween != null) scaleTween?.Kill();
        }
        public void AssignItem(Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();
        }
        public void AssignToPizza(Transform _endParent)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();
            canDrag = false;

            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(Vector3.zero, 100, 1, 0.5f)
            .OnComplete(() =>
            {
                canDrag = true;
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }
        public void OnGeneration()
        {
            startScale = transform.localScale;
            transform.localScale = Vector3.zero;

            scaleTween = transform.DOScale(startScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canDrag = true;
                isComparePos = true;
                isScaleDown = true;
            });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { pizzaTopping = this });

        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { pizzaTopping = this, backitem = this });
        }
    }
}