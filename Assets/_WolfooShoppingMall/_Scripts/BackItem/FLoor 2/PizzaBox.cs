using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class PizzaBox : BackItem
    {
        [SerializeField] Transform pizzaZone;
        [SerializeField] Image boxLidImg;
        [SerializeField] ParticleSystem smokeFxPb;

        private ParticleSystem smokeFx;
        private Vector3 startTouch;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();
            boxLidImg.gameObject.SetActive(false);
            smokeFx = Instantiate(smokeFxPb, transform);
            smokeFx.transform.localPosition = Vector3.zero;
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.pizza != null)
            {
                if (Vector2.Distance(transform.position, item.pizza.transform.position) > 2) return;

                smokeFx.Play();
            }
        }
        public void OnGeneration()
        {
            startScale = transform.localScale;
            transform.localScale = Vector3.zero;

            tweenScale = transform.DOScale(startScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canDrag = true;
                isComparePos = true;
                isScaleDown = true;
                canClick = true;
            });
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!canClick) return;

            startTouch = transform.position;

        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!canClick) return;
            if (transform.position != startTouch) return;
            canClick = false;

            smokeFx.time = 0;
            smokeFx.Play();

        }
    }
}