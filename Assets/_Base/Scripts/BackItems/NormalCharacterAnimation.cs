using Helper;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class NormalCharacterAnimation : MonoBehaviour
    {
        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("Idle")]
        [SerializeField, SpineAnimation] private string idleAnim;
        [Header("Excute")]
        [SerializeField, SpineAnimation] private string excuteAnim;

        private AnimState animState;
        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        #region Anim by Spine
        public void PlayExcute(bool isLoop = false)
        {
            if (animState == AnimState.Excute) return;
            animState = AnimState.Excute;
            SkeletonAnim.AnimationState.PlayAnimation(excuteAnim, isLoop);
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