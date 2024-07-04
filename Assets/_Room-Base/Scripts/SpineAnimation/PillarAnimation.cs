using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Helper;
using Spine.Unity;

namespace _WolfooShoppingMall
{
    public class PillarAnimation : MonoBehaviour
    {
        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("CloseIdle")]
        [SerializeField, SpineAnimation] private string closeIdleAnim;
        [Header("OpenIdle")]
        [SerializeField, SpineAnimation] private string openIdleAnim;
        [Header("Open")]
        [SerializeField, SpineAnimation] private string openAnim;
        [Header("Close")]
        [SerializeField, SpineAnimation] private string closeAnim;
        private AnimState animState;
        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        [Header("Skin")]
        [SerializeField, SpineSkin] string[] skinList;
        private Tween _tween;

        public enum ColorType
        {
            Red,
            RedDecor,
            Purple,
            Green,
            Blue,
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }

        public void ChangeSkin(ColorType colorType)
        {
            skeletonAnim.Skeleton.SetSkin(skinList[(int)colorType]);
            skeletonAnim.Skeleton.SetSlotsToSetupPose();
        }
        public void ChangeSkin()
        {
            skeletonAnim.Skeleton.SetSkin(skinList[Random.Range(0, skinList.Length)]);
            skeletonAnim.Skeleton.SetSlotsToSetupPose();
        }
     
        public void PlayOpenAnim(System.Action action = null)
        {
            PlayOpen();
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(GetTimeAnimation(animState) + 1, () =>
            {
                PlayOpenIdle();
                action?.Invoke();
            });
        }
        public void PlayCloseAnim(System.Action action = null)
        {
            PlayClose();
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(GetTimeAnimation(animState) + 1, () =>
            {
                PlayCloseIdle();
                action?.Invoke();
            });
        }
        #region Anim by Spine
        void PlayOpen()
        {
            if (animState == AnimState.Open) return;
            animState = AnimState.Open;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, openAnim, false);
        }
        void PlayClose()
        {
            if (animState == AnimState.Close) return;
            animState = AnimState.Close;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, closeAnim, false);
        }
        void PlayCloseIdle()
        {
            if (animState == AnimState.CloseIdle)
                return;
            animState = AnimState.CloseIdle;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, closeIdleAnim, true);
        }
        void PlayOpenIdle()
        {
            if (animState == AnimState.OpenIdle)
                return;
            animState = AnimState.OpenIdle;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, openIdleAnim, true);
        }
        public float GetTimeAnimation(AnimState animState)
        {
            var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closeIdleAnim);
            switch (animState)
            {
                case AnimState.CloseIdle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closeIdleAnim);
                    break;
                case AnimState.Open:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openAnim);
                    break;
                case AnimState.OpenIdle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openIdleAnim);
                    break;
            }

            float animLength = myAnimation.Duration;
            return animLength;
        }

        public enum AnimState
        {
            None,
            CloseIdle,
            Open,
            Close,
            OpenIdle
        }
        #endregion

    }

}