using DG.Tweening;
using Helper;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CurtainAnimation : MonoBehaviour
    {
        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("Closing")]
        [SerializeField, SpineAnimation] private string closingAnim;
        [Header("ClosedIdle")]
        [SerializeField, SpineAnimation] private string closedIdle;
        [Header("Opening")]
        [SerializeField, SpineAnimation] private string openingAnim;
        [Header("OpenedIdle")]
        [SerializeField, SpineAnimation] private string openedIdleAnim;
        private AnimState animState;
        private Tween _tween;

        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }
        public void Open(System.Action OnCompleted = null)
        {
            PlayOpening(false);
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                PlayOpenedIdle(true);
                OnCompleted?.Invoke();
            });
        }
        public void Close(System.Action OnCompleted = null)
        {
            PlayClosing(false);
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                PlayClosedIdle(true);
                OnCompleted?.Invoke();
            });
        }

        #region Anim by Spine
        void PlayClosedIdle(bool state)
        {
            if (closedIdle == null) return;
            if (animState == AnimState.ClosedIdle) return;
            animState = AnimState.ClosedIdle;
            //if (SoundManager.instance != null)
            //    SoundManager.instance.PlayWolfooSfx(SfxWolfooType.Walk, sexType);
            SkeletonAnim.AnimationState.PlayAnimation(closedIdle, state);
        }
        void PlayClosing(bool state)
        {
            if (closingAnim == null) return;
            if (animState == AnimState.Closing)
                return;
            animState = AnimState.Closing;
            SkeletonAnim.AnimationState.PlayAnimation(closingAnim, state);
        }
        void PlayOpening(bool state)
        {
            if (openingAnim == null) return;
            if (animState == AnimState.Opening)
                return;
            //    SoundManager.instance.PlayWolfooSfx(SfxWolfooType.Complain);
            animState = AnimState.Opening;
            SkeletonAnim.AnimationState.PlayAnimation(openingAnim, state);
        }
        void PlayOpenedIdle(bool state)
        {
            if (openedIdleAnim == null) return;
            if (animState == AnimState.OpenedIdle)
                return;
            //    SoundManager.instance.PlayWolfooSfx(SfxWolfooType.Complain);
            animState = AnimState.OpenedIdle;
            SkeletonAnim.AnimationState.PlayAnimation(openedIdleAnim, state);
        }
        public float GetTimeAnimation(AnimState animState)
        {
            var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closingAnim);
            switch (animState)
            {
                case AnimState.Closing:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closingAnim);
                    break;
                case AnimState.ClosedIdle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closedIdle);
                    break;
                case AnimState.Opening:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openingAnim);
                    break;
                case AnimState.OpenedIdle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openedIdleAnim);
                    break;
            }

            float animLength = myAnimation.Duration;
            return animLength;
        }

        public enum AnimState
        {
            None,
            Closing,
            ClosedIdle,
            Opening,
            OpenedIdle,
        }
        #endregion

    }
}