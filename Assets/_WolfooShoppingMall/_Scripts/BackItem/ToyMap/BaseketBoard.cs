using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class BaseketBoard : BackItem
    {
        [SerializeField] Transform itemZone;
        [SerializeField] Transform itemZone2;
        private float distance;

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
            if (item.ball == null) return;

            distance = Vector2.Distance(item.ball.transform.position, itemZone.position);
            if (distance > 2) return;

            CheckPriority(() =>
            {
                item.ball.JumpToBasket(itemZone.localPosition, itemZone2.localPosition, itemZone, () =>
                {
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.BasketBall);
                },
                () =>
                {
                });

                if (delayTween != null) delayTween?.Kill();
                if (tweenPunch != null)
                {
                    transform.localScale = startScale;
                    tweenPunch?.Kill();
                }
                delayTween = DOVirtual.DelayedCall(0.5f, () =>
                {
                    tweenPunch = transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.25f, 1);
                });
            });
        }
    }
}