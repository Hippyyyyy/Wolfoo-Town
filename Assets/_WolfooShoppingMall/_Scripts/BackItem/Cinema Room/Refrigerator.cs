using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using SCN.Common;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Refrigerator : BackItem
    {
        [SerializeField] Transform endTrans;
        [SerializeField] List<Sprite> itemSprites;
        [SerializeField] List<Image> itemImages;
        [SerializeField] ItemInRefrigerator itemPb;
        private int maxItem;
        private Tweener rotateTween;

        protected override void Start()
        {
            base.Start();

            maxItem = itemSprites.Count;
        }
        private void OnDestroy()
        {
            if (rotateTween != null) rotateTween?.Kill();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            if (rotateTween == null) rotateTween?.Kill();
            transform.localScale = startScale;
            rotateTween = transform.DOPunchScale(new Vector2(-0.1f, 0.1f), 0.5f, 2).OnComplete(() =>
            {
                canClick = true;
            });

            int rd = UnityEngine.Random.Range(0, itemSprites.Count);
            var newObject = Instantiate(itemPb, endTrans);
            newObject.transform.position = itemImages[rd].transform.position;
            newObject.AssignItem(itemImages[rd].sprite);
            newObject.transform.DOMoveY(endTrans.position.y, 1).OnComplete(() =>
            {
                newObject.transform.SetParent(endTrans.parent.parent);
                newObject.AssignItem();
            });
        }

        protected override void InitItem()
        {
            canClick = true;
        }
    }
}
