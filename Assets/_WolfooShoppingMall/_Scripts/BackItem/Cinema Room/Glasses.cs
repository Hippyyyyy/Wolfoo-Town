using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Glasses : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            isScaleDown = true;
            IsCarry = true;
            isComparePos = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { glasses = this, backitem = this });
        }

        public void OnHang(Transform _endParent, bool isWolfoo = true)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            if (!isWolfoo)
            {
                KillScalling();
                transform.localScale = Vector3.one * 3;
            }
        }
    }
}