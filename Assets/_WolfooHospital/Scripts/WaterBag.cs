using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class WaterBag : TransformBackItem
    {
        [SerializeField] Transform compareTrans;
        private Vector3 awakePos;
        private Transform awakeParent;

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;

            awakePos = transform.localPosition;
            awakeParent = transform.parent;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if(Vector2.Distance(transform.position, compareTrans.position) < 1)
            {
                IsAssigned = true;
                isTransform = true;
                SetState();
                KillDragging();
                transform.SetParent(awakeParent);
                transform.localPosition = awakePos;
            }
            else
            {
                isTransform = false;
                SetState();
            }
        }
    }
}