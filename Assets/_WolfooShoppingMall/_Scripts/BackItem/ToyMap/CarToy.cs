using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CarToy : BackItem
    {
        private int count;

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
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, carToy = this });
        }

        //public void OnSlide(Transform locationZone, Transform parent)
        //{
        //    canMoveToGround = false;
        //    canDrag = false;
        //    KillDragging();

        //    if(count == 0)
        //    {
        //        transform.SetParent(parent);
        //        transform.localPosition = locationZone.GetChild(0).localPosition;
        //    }

        //    transform.DORotate(locationZone.GetChild(count).rotation.eulerAngles, 0.5f).SetEase(Ease.Linear);
        //    transform.DOLocalMove(locationZone.GetChild(count).localPosition, 800)
        //        .SetEase(Ease.Linear)
        //        .SetSpeedBased(true)
        //        .OnComplete(() =>
        //        {
        //            count++;
        //            if (count == locationZone.childCount - 1)
        //            {
        //                transform.DORotate(locationZone.GetChild(count).rotation.eulerAngles, 0.5f).SetEase(Ease.Linear);
        //                transform.DOLocalMove(locationZone.GetChild(count).localPosition, 1000)
        //                   .SetEase(Ease.Linear)
        //                   .SetSpeedBased(true).OnComplete(() =>
        //                   {
        //                       count = 0;
        //                       canMoveToGround = true;
        //                       canDrag = true;
        //                   });
        //            }
        //            else
        //            {
        //                OnSlide(locationZone, parent);
        //            }
        //        });
        //}
    }
}