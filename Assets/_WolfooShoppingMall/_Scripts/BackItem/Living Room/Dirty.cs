using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Dirty : BackItem
    {
        [SerializeField] ParticleSystem lightingFx;
        private Tweener fadeTween;
        private bool isCleaned;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();

        }
        protected override void GetDragItem(EventKey.OnDragBackItem item)
        {
            base.GetDragItem(item);
            if (isCleaned) return;
            if (item.mop == null) return;

            if (Vector2.Distance(item.mop.CleanZone.position, transform.position) <= 3)
            {
                if (fadeTween != null && fadeTween.IsActive()) return;
                fadeTween = image.DOFade(0.3f, 0.5f).SetSpeedBased(true).OnComplete(() =>
                {
                    image.DOFade(0f, 0.2f);
                    isCleaned = true;
                    lightingFx.Play();
                });
            }
            else
            {
                fadeTween?.Kill();
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (isCleaned) return;
            if (item.mop == null) return;
            fadeTween?.Kill();
        }
    }
}