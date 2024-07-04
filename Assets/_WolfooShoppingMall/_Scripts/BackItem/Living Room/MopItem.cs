using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using SCN;

namespace _WolfooShoppingMall
{
    public class MopItem : BackItem
    {
        [SerializeField] Transform cleanZone;

        public Transform CleanZone { get => cleanZone; }

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();
            isComparePos = true;
            canDrag = true;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            GameManager.instance.GetCurrentPosition(transform);
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragBackItem { backItem = this, mop = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, mop = this });

            MoveToStartPos();
        }
    }
}