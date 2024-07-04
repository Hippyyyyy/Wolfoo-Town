using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class TowerCake : BackItem
    {
        [SerializeField] Image[] creamImgs;
        private float distance;
        private int lastIdx;
        private int curIdx = -1;
        private float curDistance;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.cream != null)
            {
                lastIdx = curIdx;
                curIdx = -1;
                curDistance = 1000;

                for (int i = 0; i < creamImgs.Length; i++)
                {
                    distance = Vector2.Distance(creamImgs[i].transform.position, item.cream.CompareZone.position);
                    if (distance < 1)
                    {
                        if (distance < curDistance)
                        {
                            curIdx = i;
                            curDistance = distance;
                        }
                    }
                }

                if (curIdx == -1) return;

                if (tweenScale != null) tweenScale?.Kill();
                if (lastIdx != -1)
                    creamImgs[lastIdx].transform.localScale = Vector3.one;

                creamImgs[curIdx].transform.localScale = Vector3.zero;
                item.cream.OnMakingCream(creamImgs[curIdx].transform.position, () =>
                {
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.PourCream);
                    creamImgs[curIdx].sprite = data.CakeData.creamCakeSprites[item.cream.IdItem];
                    tweenScale = creamImgs[curIdx].transform.DOScale(1, 0.5f);
                });
            }
        }
    }
}