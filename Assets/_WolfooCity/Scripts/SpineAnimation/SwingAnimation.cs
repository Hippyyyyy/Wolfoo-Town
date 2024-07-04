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
    public class SwingAnimation : MonoBehaviour
    {
        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("ExcuteBoth")]
        [SerializeField, SpineAnimation] private string excuteBothAnim;
        [Header("ExcuteLeft")]
        [SerializeField, SpineAnimation] private string excuteLeftAnim;
        [Header("ExcuteRight")]
        [SerializeField, SpineAnimation] private string excuteRightAnim;
        [Header("Idle")]
        [SerializeField, SpineAnimation] private string idleAnim;

        private AnimState animState;
        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        [Header("Skin")]
        [SerializeField, SpineSkin] string[] skinList;

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


        #region Anim by Spine
        public void PlayExcuteRight()
        {
            if (animState == AnimState.ExcuteRight) return;
            animState = AnimState.ExcuteRight;
            SkeletonAnim.AnimationState.PlayAnimation(excuteRightAnim, true);
            SoundManager.instance.PlayLoopingSfx(SfxOtherType.Swinging);
        }
        public void PlayIdle()
        {
            if (animState == AnimState.Idle) return;
            animState = AnimState.Idle;
            SkeletonAnim.AnimationState.PlayAnimation(idleAnim, true);
            SoundManager.instance.TurnOffLoop();
        }
        public void PlayExcuteBoth()
        {
            if (animState == AnimState.ExcuteBoth)
                return;
            animState = AnimState.ExcuteBoth;
            SkeletonAnim.AnimationState.PlayAnimation(excuteBothAnim, true);
            SoundManager.instance.PlayLoopingSfx(SfxOtherType.Swinging);
        }
        public void PlayExcuteLeft()
        {
            if (animState == AnimState.ExcuteLeft)
                return;
            animState = AnimState.ExcuteLeft;
            SkeletonAnim.AnimationState.PlayAnimation(excuteLeftAnim, true);
            SoundManager.instance.PlayLoopingSfx(SfxOtherType.Swinging);
        }

        public float GetTimeAnimation(AnimState animState)
        {
            var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excuteBothAnim);
            switch (animState)
            {
                case AnimState.ExcuteBoth:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excuteBothAnim);
                    break;
                case AnimState.ExcuteRight:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excuteRightAnim);
                    break;
                case AnimState.Idle:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excuteLeftAnim);
                    break;
                case AnimState.ExcuteLeft:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(idleAnim);
                    break;
            }

            float animLength = myAnimation.Duration;
            return animLength;
        }

        public enum AnimState
        {
            None,
            ExcuteBoth,
            ExcuteRight,
            Idle,
            ExcuteLeft,
        }
        #endregion

    }
}