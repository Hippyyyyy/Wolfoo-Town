using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class PaperBox : BackItem
    {
        [SerializeField] ToiletPaper paperPb;
        [SerializeField] Transform paperZone;
        private Tween _tween;
        private ToiletPaper myPaper;

        protected override void InitData()
        {
            base.InitData();
            myPaper = Instantiate(paperPb, paperZone);
            myPaper.Spawn();
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.paper != null && myPaper == item.paper )
            {
                _tween?.Kill();
                _tween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    myPaper = Instantiate(paperPb, paperZone);
                    myPaper.Spawn();
                });
            }
        }
    }
}