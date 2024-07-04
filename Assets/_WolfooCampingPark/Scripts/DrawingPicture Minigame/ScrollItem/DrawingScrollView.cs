using DG.Tweening;
using SCN;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class DrawingScrollView : VerticalScrollInfinity
    {
        [SerializeField] Sprite[] subItemSprites;
        [SerializeField] Color[] subItemColors;

        private void OnEnable()
        {
            DrawingItem.OnBeginDragAct += OnBeginDragItem;
            DrawingItem.OnDragAct += OnDragItem;
            DrawingItem.OnEndDragAct += OnEndDragItem;
        }
        private void OnDisable()
        {
            DrawingItem.OnBeginDragAct -= OnBeginDragItem;
            DrawingItem.OnDragAct -= OnDragItem;
            DrawingItem.OnEndDragAct -= OnEndDragItem;
        }

        private void OnBeginDragItem(DrawingItem obj)
        {
            StopAutoMove();
        }

        private void OnDragItem(DrawingItem obj)
        {
        }

        private void OnEndDragItem(DrawingItem obj)
        {
            PlayAutoMove();
        }

        protected override void GetInitItem(ScrollItemBase scrollItem)
        {
            base.GetInitItem(scrollItem);

            var drawingItem = scrollItem.GetComponent<DrawingScrollItem>();
            if (drawingItem != null)
            {
                drawingItem.Assign(subItemSprites[drawingItem.Order]);
                if (subItemColors.Length > 0)
                    drawingItem.Assign(subItemColors[drawingItem.Order]);
            }
        }
    }
}
