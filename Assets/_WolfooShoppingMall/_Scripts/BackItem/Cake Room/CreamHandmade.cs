using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CreamHandmade : BackItem
    {
        [SerializeField] Transform compareZone;
        [SerializeField] CreamColor color;
        private Tweener rotateTween;

        private int idItem;

        public enum CreamColor
        {
            Yellow,
            Purple,
            Pink,
            Brown,
            Green
        }

        public Transform CompareZone { get => compareZone; }
        public int IdItem { get => idItem; }

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
        protected override void InitData()
        {
            base.InitData();
            for (int i = 0; i < data.CakeData.colors.Length; i++)
            {
                if (data.CakeData.colors[i] == color)
                {
                    idItem = i;
                    break;
                }
            }
        }

        public void OnMakingCream(Vector3 _endPos, System.Action OnComplete)
        {
            OnComplete?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.forward * 90, 0.3f);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.zero, 0.3f);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { cream = this, backitem = this });
        }
    }
}