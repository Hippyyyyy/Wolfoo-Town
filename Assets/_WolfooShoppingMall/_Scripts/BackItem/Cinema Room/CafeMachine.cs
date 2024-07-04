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
    public class CafeMachine : BackItem
    {
        [SerializeField] List<Transform> waitingZones;
        [SerializeField] List<ParticleSystem> beverageFxs;

        private int maxItem;
        private Tweener shakeTween;
        private int shakeCount;
        private float distance;
        private Transform verifiedTrans;

        public enum BeverageColor
        {
            Brown,
            Green,
            Yellow
        }

        private void OnDestroy()
        {
            if (shakeTween != null) shakeTween?.Kill();
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.plasticup != null)
            {
                if (item.plasticup.IsHasWater) return;

                OnCompare(item.plasticup, () =>
                {
                    int idxVerified = waitingZones.IndexOf(verifiedTrans);
                    item.plasticup.OnGetBeverage(2, verifiedTrans.position, idxVerified, verifiedTrans);

                    beverageFxs[idxVerified].Play();
                    delayTween = DOVirtual.DelayedCall(2, () =>
                    {
                        beverageFxs[idxVerified].Stop();
                    });

                    if (shakeTween == null) shakeTween?.Kill();
                    transform.localPosition = startLocalPos;
                    shakeCount = 0;

                    OnShake();
                }, null);
            }
        }

        void OnCompare(BackItem item, Action OnSuccess, Action OnFail)
        {
            verifiedTrans = null;
            float minDistance = 0;

            foreach (var waitingZone in waitingZones)
            {
                distance = Vector2.Distance(item.StandZone.position, waitingZone.position);
                if (distance <= 1)
                {
                    if (verifiedTrans == null)
                    {
                        minDistance = distance;
                        verifiedTrans = waitingZone;
                        continue;
                    }

                    if (distance < minDistance)
                        verifiedTrans = waitingZone;
                }
            }

            if (verifiedTrans != null)
            {
                OnSuccess?.Invoke();
            }
            else
            {
                OnFail?.Invoke();
            }
        }

        void OnShake()
        {
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Blender);
            shakeCount++;
            if (shakeCount == 4)
            {
                shakeTween = transform.DOLocalMove(startLocalPos, 0.1f)
                    .SetEase(Ease.Flash)
                    .OnComplete(() =>
                    {
                        canClick = true;
                    });
                return;
            }
            shakeTween = transform.DOLocalMove(startLocalPos + new Vector3(5, 5, 0), 0.1f)
                .SetEase(Ease.Flash)
                .OnComplete(() =>
                {
                    shakeTween = transform.DOLocalMove(startLocalPos + new Vector3(-5, -5, 0), 0.1f)
                      .SetEase(Ease.Flash)
                      .OnComplete(() =>
                      {
                          OnShake();
                      });
                });
        }

        protected override void InitItem()
        {
        }
    }
}