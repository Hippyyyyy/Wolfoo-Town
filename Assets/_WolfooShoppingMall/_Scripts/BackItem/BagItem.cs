using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using SCN;

namespace _WolfooShoppingMall
{
    public class BagItem : BackItem
    {
        [SerializeField] List<Sprite> sprites;
        [SerializeField] 
        int curIdx = 0;
        private float distance_;

        int count = 0;
        List<BackItem> curItems = new List<BackItem>();
        private Vector2 range;
        private bool isItemJumpOutSide;

        protected override void InitItem()
        {
            isClick = true;
            isStandTable = true;
            canDrag = true;
        }

        protected override void Start()
        {
            base.Start();
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            count = 0;
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null || item.backitem == this) return;
            if (item.character != null) return;

        //    if (curIdx == 0) return;
         //   if (!item.backitem.IsInBag) return;

            distance_ = Vector2.Distance(item.backitem.transform.position, transform.position);
            if (distance_ <= 1)
            {
                item.backitem.transform.SetParent(transform);
                item.backitem.JumpToEndLocalPos(Vector3.zero, () =>
                {
                    item.backitem.gameObject.SetActive(false);
                });
                curItems.Add(item.backitem);
            }
            else
            {
                //     item.backitem.MoveToGround();
            }
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, bag = this });
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (!isItemJumpOutSide)
            {
                range = new Vector2(transform.position.x - 4, transform.position.x + 4);
                count++;
                if (count > 2)
                {
                    count = 0;
                    if (curItems.Count > 0)
                    {
                        isItemJumpOutSide = true;
                        foreach (var item in curItems)
                        {
                            float rd = UnityEngine.Random.Range(range.x, range.y);

                            item.gameObject.SetActive(true);
                            item.transform.SetParent(Content.transform);
                            item.JumpToEndPos(new Vector3(rd, transform.position.y, 0), transform.parent);
                            item.transform.localScale = Vector3.one;
                        }
                        curItems.Clear();
                        isItemJumpOutSide = false;
                    }
                }
            }

            OnPunchScale();
            curIdx = 1 - curIdx;
            image.sprite = sprites[curIdx];
            image.SetNativeSize();
        }
    }
}