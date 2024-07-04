using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Food : BackItem
    {
        [SerializeField] bool isAssignStart = true;
        [SerializeField] bool isCandy;
        [SerializeField] bool isGrill;

        public bool IsMeat { get; private set; }

        private MeatData meatData;
        private int spawnIdx = -1;
        private Transform awakeParent;

        public bool IsCandy { get => isCandy; }
        public Transform AwakeParent { get => awakeParent; }
        public bool IsGrill { get => isGrill; }

        protected override void InitItem()
        {
            if (!isAssignStart) return;

            canDrag = true;
            IsFood = true;
            isComparePos = true;
            isScaleDown = true;
            startScale = transform.localScale;
            IsCarry = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, food = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { backitem = this, food = this });
        }

        public override void AssignDrag()
        {
            base.AssignDrag();
            canDrag = true;
            IsFood = true;
            isComparePos = true;
            isScaleDown = true;
            startScale = transform.localScale;
            IsCarry = true;
        }

        public void OnGrilling()
        {
            if (!IsMeat) return;
            image.sprite = meatData.grilledSprt;
            image.SetNativeSize();
        }

        public void AssignMeat(MeatData _data)
        {
            IsMeat = true;
            meatData = _data;
            image.sprite = meatData.idleSprt;
            image.SetNativeSize();

            AssignDrag();
        }

        public void Setup(int spawnIdx, Sprite sprite = null)
        {
            this.spawnIdx = spawnIdx;
            awakeParent = transform.parent;

            if (sprite != null)
            {
                image.sprite = sprite;
                image.SetNativeSize();
            }
            transform.localPosition = Vector3.zero;
        }
    }

}