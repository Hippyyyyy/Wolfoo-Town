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
    public class PlasticBubbleAnimation : MonoBehaviour, IPointerDownHandler
    {
        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("Idle")]
        [SerializeField, SpineAnimation] private string idleAnim;
        [Header("Excute")]
        [SerializeField, SpineAnimation] private string excuteAnim;

        private AnimState animState;
        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        [Header("Skin")]
        [SerializeField, SpineSkin] string[] skinList;
        private Tween delayTween;
        private bool canClick = true;

        public enum ColorType
        {
            Red,
            RedDecor,
            Purple,
            Green,
            Blue,
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!canClick) return;
            canClick = false;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Scratch);
            PlayExcute();
            if (delayTween != null) delayTween?.Kill();
            delayTween = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.Excute), () =>
            {
                PlayIdle();
                canClick = true;
            });
        }

        #region Anim by Spine
        public void PlayExcute()
        {
            if (animState == AnimState.Excute) return;
            animState = AnimState.Excute;
            SkeletonAnim.AnimationState.PlayAnimation(excuteAnim, false);
        }
        public void Stop()
        {
            PlayIdle();
        }
        public void PlayIdle()
        {
            if (animState == AnimState.Idle)
                return;
            animState = AnimState.Idle;
            SkeletonAnim.AnimationState.PlayAnimation(idleAnim, true);
        }

        public float GetTimeAnimation(AnimState animState)
        {
            var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(idleAnim);
            switch (animState)
            {
                case AnimState.Idle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(idleAnim);
                    break;
                case AnimState.Excute:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excuteAnim);
                    break;
            }

            float animLength = myAnimation.Duration;
            return animLength;
        }

        public enum AnimState
        {
            None,
            Idle,
            Excute,
        }
        #endregion
    }
}