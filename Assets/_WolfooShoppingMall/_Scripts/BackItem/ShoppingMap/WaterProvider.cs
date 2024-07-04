using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace _WolfooShoppingMall
{
    public class WaterProvider : BackItem
    {
        [SerializeField] ParticleSystem waterFx;
        [SerializeField] Transform endWaterZone;
        [SerializeField] Animator animator;
        [SerializeField] string playName;
        private bool isPouring;
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> myRotateTween;

        public Action OnPourComplete { get; private set; }
        public bool IsNewModel
        {
            get => animator != null;
        }
        public Transform WaterPouringArea
        {
            get => endWaterZone;
        }

        protected override void InitItem()
        {
            canDrag = true;
            isScaleDown = true;
            if (animator != null) animator.enabled = false;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            waterFx.gameObject.SetActive(false);
            myRotateTween?.Kill();
            myRotateTween = transform.DORotate(Vector3.zero, 0.25f);

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { waterProvider = this, backitem = this });
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragBackItem { waterProvider = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            startPos = transform.position;

            if (IsNewModel)
            {
                animator.enabled = false;
                myRotateTween?.Kill();
                myRotateTween = transform.DORotate(Vector3.forward * 50, 0.25f).OnComplete(() =>
                {
                    waterFx.gameObject.SetActive(true);
                });
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (myRotateTween != null) myRotateTween?.Kill();
        }

        public void OnAnimPouringComplete()
        {
            canDrag = true;
            animator.enabled = false;
            waterFx.Stop();
            OnPourComplete?.Invoke();
        }

        public void PouringWater(Vector3 endPos, System.Action OnComplete)
        {
            IsAssigned = true;
            KillDragging();

            canDrag = false;
            OnPourComplete = OnComplete;

            tweenMove = transform.DOMove(endPos, 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.enabled = true;
                animator.Play(playName, 0, 0);
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Watering);
            });
        }
        public void OnPourWater(Vector3 _endPos, Transform _endParent, System.Action OnComplete)
        {
            if (isPouring) return;

            isPouring = true;
            canDrag = false;
            canMoveToGround = false;
            KillDragging();
            IsAssigned = true;

            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(_endPos - endWaterZone.localPosition, 200, 1, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                waterFx.gameObject.SetActive(true);
                waterFx.Play();
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Watering);
                transform.DORotate(Vector3.forward * 70, 0.5f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        isPouring = false;
                        waterFx.Stop();
                        OnComplete?.Invoke();

                        transform.DOJump(startPos, 1, 1, 0.5f);
                        transform.DORotate(Vector3.zero, 0.5f).OnComplete(() =>
                        {
                            canDrag = true;
                        });
                    });
                });
            });
        }
    }
}