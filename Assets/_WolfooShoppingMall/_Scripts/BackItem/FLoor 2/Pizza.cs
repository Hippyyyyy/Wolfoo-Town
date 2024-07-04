using DG.Tweening;
using SCN;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Pizza : BackItem
    {
        [SerializeField] ParticleSystem smokeFx;
        [SerializeField] Transform boxZone;
        [SerializeField] Transform toppingZone;
        [SerializeField] bool isAssign;

        private Vector3 startTouch;
        private RandomNoRepeat<int> rdIDxNrp;
        private bool isPacking;

        public bool IsPacked { get; private set; }

        protected override void InitItem()
        {
            if (isAssign)
            {
                canDrag = true;
                isComparePos = true;
                isScaleDown = true;
            }
            IsFood = true;
            IsScratch = true;
        }
        protected override void Start()
        {
            base.Start();
            boxZone.gameObject.SetActive(false);

            var rdIdxs = new List<int>();
            for (int i = 0; i < toppingZone.childCount; i++)
            {
                rdIdxs.Add(i);
            }
            rdIDxNrp = new RandomNoRepeat<int>(rdIdxs);

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { pizza = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { pizza = this, backitem = this });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.pizzaTopping != null)
            {
                if (Vector2.Distance(item.pizzaTopping.transform.position, toppingZone.position) > 2) return;
                item.pizzaTopping.AssignToPizza(toppingZone.GetChild(rdIDxNrp.Random()));
            }
        }
        public void AssignItem(Sprite sprite)
        {
            image.sprite = sprite;
            image.SetNativeSize();
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
            });
        }
        public void OnPack(Vector3 _endPos)
        {
            if (isPacking) return;
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            isPacking = true;
            transform.position = _endPos;
            smokeFx.time = 0;
            smokeFx.Play();

            canClick = true;
            boxZone.gameObject.SetActive(true);
            toppingZone.transform.localScale = Vector3.zero;

            image.enabled = false;
        }
        public void OnGrilled(Vector3 _endPos, Sprite grillSprite)
        {
            if (isPacking) return;

            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            smokeFx.time = 0;
            smokeFx.Play();

            //    toppingZone.transform.localScale = Vector3.zero;

            image.sprite = grillSprite;
            image.SetNativeSize();
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

            isPacking = false;
            IsPacked = true;

            smokeFx.time = 0;
            smokeFx.Play();
            image.enabled = true;
            boxZone.gameObject.SetActive(false);
            toppingZone.transform.localScale = Vector3.one;
        }
    }
}