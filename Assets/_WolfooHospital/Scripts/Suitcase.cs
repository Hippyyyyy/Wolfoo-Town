using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Suitcase : BackItem
    {
        [SerializeField] Transform[] limitZones;
        [SerializeField] Transform itemZone;
        [SerializeField] Animator animator;

        bool isOpen;
        private Edge[] myEdges;

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isComparePos = true;
            canClick = true;

            myEdges = GameManager.instance.GetEdges(limitZones);
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (!isOpen) return;
            BackItem verifiedItem = null;
            if (item.food != null) verifiedItem = item.food;
            if (item.toy != null) verifiedItem = item.toy;

            if (verifiedItem == null) return;

            if (Vector2.Distance(verifiedItem.transform.position, itemZone.position) < 3)
            {
                if (!GameManager.instance.Is_inside(verifiedItem.transform.position, myEdges)) return;
                verifiedItem.transform.SetParent(itemZone);
                verifiedItem.JumpToEndLocalPos(verifiedItem.transform.localPosition, null, DG.Tweening.Ease.OutBounce, 50, false, assignPriorityy);
            }
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            isOpen = !isOpen;

            if (isOpen)
            {
                animator.SetTrigger("Open");
                itemZone.gameObject.SetActive(true);
            }
            else
            {
                animator.SetTrigger("Close");
                itemZone.gameObject.SetActive(false);
            }
        }
    }
}