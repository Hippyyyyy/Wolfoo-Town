using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class FaceMaskBox : BackItem
    {
        [SerializeField] FaceMask faceMaskPb;
        [SerializeField] Transform faceMaskZone;
        private Tween _delayTween;
        private FaceMask myFaceMask;

        protected override void InitData()
        {
            base.InitData();
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if(item.faceMask != null)
            {
                Debug.Log("Check FaceMask");
                if (myFaceMask != null && item.faceMask != myFaceMask) return;
                Debug.Log("Instatiate FaceMask");
                _delayTween?.Kill();
                _delayTween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    myFaceMask = Instantiate(faceMaskPb, faceMaskZone);
                Debug.Log("Spawn FaceMask");
                    myFaceMask.Spawn();
                });
            }
        }
    }
}