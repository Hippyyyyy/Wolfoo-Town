using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class OperaHat : Hat
    {
        private OperaHatPulledPackage myPackageA;

        public OperaHatPulledPackage Package { get => myPackageA; }

        protected override void InitData()
        {
            base.InitData();
            IsNormalHat = false;
        }
        public void AssignItem(OperaHatPulledPackage packageArea)
        {
            myPackageA = packageArea;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, operaHat = this });
        }
        public void Swear2(Transform _parent, bool isFlip, bool isWOlfoo = true)
        {
            IsAssigned = true;
            KillDragging();
            KillScalling();
            transform.SetParent(_parent);
            transform.localPosition = wearingLocalPos;
            transform.localRotation = Quaternion.Euler(isFlip ? Vector3.up * 180 : Vector3.zero);
            if (isWOlfoo)
                transform.localScale = Vector3.one;
            else 
                transform.localScale = Vector3.one * 3;
        }
    }
}