using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterWolfooWorldAnimation : MonoBehaviour
    {
        [Header("Skin")]
        [SerializeField, SpineSkin] string[] skinList;

        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonAnimation skeletonAnim;
        [Header("Idle")]
        [SerializeField, SpineAnimation] private string idleAnim;
        [Header("Run")]
        [SerializeField, SpineAnimation] private string runAnim;
        [Header("Walk")]
        [SerializeField, SpineAnimation] private string walkAnim;
        [Header("Laugh")]
        [SerializeField, SpineAnimation] private string laughAnim;
        [Header("Jump")]
        [SerializeField, SpineAnimation] private string jumpAnim;
        [Header("Happy")]
        [SerializeField, SpineAnimation] private string happyAnim;
        [Header("Sad")]
        [SerializeField, SpineAnimation] private string sadAnim;
        [Header("Special")]
        [SerializeField, SpineAnimation] private string specialAnim;
        [Header("WaveHand")]
        [SerializeField, SpineAnimation] private string wavehandAnim;
        [Header("Eat")]
        [SerializeField, SpineAnimation] private string EatAnim;
        [Header("Disagree")]
        [SerializeField, SpineAnimation] private string DisagreeAnim;
        [Header("Dizzy")]
        [SerializeField, SpineAnimation] private string dizzyAnim;
        [Header("Sit")]
        [SerializeField, SpineAnimation] private string sitAnim;
        [Header("TakeAPhoto")]
        [SerializeField, SpineAnimation] private string takeAPhoto;
        [Header("Sleep")]
        [SerializeField, SpineAnimation] private string sleepAnim;
        private bool isSit;
        public AnimState animState;
        private Tween tweenDelay;

        public enum SexType
        {
            Boy,
            Girl,
        }

        public SkeletonAnimation SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        public enum SkinType
        {
            One,
            Two,
            Three,
            Four,
            Five,
        }

        private void OnDestroy()
        {
            if (tweenDelay != null) tweenDelay?.Kill();
        }

        public void ChangeSkin(SkinType colorType)
        {
            skeletonAnim.Skeleton.SetSkin(skinList[(int)colorType]);
            skeletonAnim.Skeleton.SetSlotsToSetupPose();
        }
        public void ChangeSkin(int idx)
        {
            if (skinList == null) return;
            if (idx >= skinList.Length) return;

            skeletonAnim.Skeleton.SetSkin(skinList[idx]);
            skeletonAnim.Skeleton.SetSlotsToSetupPose();
        }
        public void ChangeSkin()
        {
            if (skinList == null) return;
            if (skinList.Length == 0) return;
            skeletonAnim.Skeleton.SetSkin(skinList[Random.Range(0, skinList.Length)]);
            skeletonAnim.Skeleton.SetSlotsToSetupPose();
        }

        void PlaySound(AnimState animState)
        {
            switch (animState)
            {
                case AnimState.Run:
                    break;
                case AnimState.Jump:
                    break;
                case AnimState.Happy:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hooray);
                    break;
                case AnimState.Sad:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Sad);
                    break;
                case AnimState.Special:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hoow);
                    break;
                case AnimState.WaveHand:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hello);
                    break;
                case AnimState.Laugh:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Laugh);
                    break;
                case AnimState.Walk:
                    break;
                case AnimState.Eat:
                    SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Eating1);
                    break;
                case AnimState.Disagree:
                    break;
                case AnimState.Dizzy:
                    break;
                case AnimState.Sit:
                    break;
                case AnimState.TakeAPhoto:
                    break;
                case AnimState.Sleep:
                    break;
            }
        }

        #region Anim by Spine
        public void PlayMove()
        {
            if (runAnim == null) return;
            if (animState == AnimState.Run) return;
            animState = AnimState.Run;
            SkeletonAnim.AnimationState.SetAnimation(0, runAnim, true);
            PlaySound(animState);
        }
        public void PlayIdle()
        {
            if (idleAnim == null) return;
            if (animState == AnimState.Idle)
                return;
            isSit = false;
            animState = AnimState.Idle;
            SkeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
            PlaySound(animState);
        }
        public void PlaySit()
        {
            if (sitAnim == null) return;
            if (animState == AnimState.Sit)
                return;
            isSit = true;
            animState = AnimState.Sit;
            SkeletonAnim.AnimationState.SetAnimation(0, sitAnim, true);
            PlaySound(animState);
        }
        public void PlayDizzy()
        {
            if (dizzyAnim == null) return;
            if (animState == AnimState.Dizzy)
                return;
            animState = AnimState.Dizzy;
            SkeletonAnim.AnimationState.SetAnimation(0, dizzyAnim, true);
            PlaySound(animState);
        }
        public void PLayTakeAPhoto(bool isLoop = false)
        {
            if (takeAPhoto == null) return;
            animState = AnimState.TakeAPhoto;
            SkeletonAnim.AnimationState.SetAnimation(0, takeAPhoto, isLoop);
            PlaySound(animState);

            if (!isLoop)
            {
                tweenDelay?.Kill();
                tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
                {
                    if (isSit) { PlaySit(); }
                    else { PlayIdle(); }
                });
            }
        }

        public void PlaySleep(bool isLoop = true)
        {
            if (animState == AnimState.Sleep)
                return;
            animState = AnimState.Sleep;
            SkeletonAnim.AnimationState.SetAnimation(0, sleepAnim, isLoop);
            PlaySound(animState);
        }

        public void PlayJump(bool isLoop = false)
        {
            if (jumpAnim == null) return;
            if (animState == AnimState.Jump)
                return;
            animState = AnimState.Jump;
            SkeletonAnim.AnimationState.SetAnimation(0, jumpAnim, isLoop);
            PlaySound(animState);

            if (!isLoop)
            {
                if (tweenDelay != null) tweenDelay?.Kill();
                tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
                {
                    if (isSit)
                    {
                        PlaySit();
                        return;
                    }
                    PlayIdle();
                });
            }
        }
        public void PlayDisagree()
        {
            if (DisagreeAnim == null) return;
            if (animState == AnimState.Disagree)
                return;
            animState = AnimState.Disagree;
            SkeletonAnim.AnimationState.SetAnimation(0, DisagreeAnim, false);
            PlaySound(animState);

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                if (isSit)
                {
                    PlaySit();
                    return;
                }
                PlayIdle();
            });
        }
        public void PlayEat()
        {
            if (EatAnim == null) return;
            PlayIdle();

            if (animState == AnimState.Eat)
                return;
            animState = AnimState.Eat;
            SkeletonAnim.AnimationState.SetAnimation(0, EatAnim, false);
            PlaySound(animState);

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                if (isSit)
                {
                    PlaySit();
                    return;
                }
                PlayIdle();
            });
        }
        public void PlayEat(bool isCandy)
        {
            if (EatAnim == null) return;
            if (animState == AnimState.Eat)
                return;
            animState = AnimState.Eat;
            SkeletonAnim.AnimationState.SetAnimation(0, EatAnim, false);

            SoundCharacterManager.Instance.Play(isCandy ? SoundCharacterManager.SfxWolfooType.Eating1 : SoundCharacterManager.SfxWolfooType.Eating2);

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Delicious);
                if (isSit)
                {
                    PlaySit();
                    return;
                }
                PlayIdle();
            });
        }
        public void PlayHappy()
        {
            if (happyAnim == null) return;
            if (animState == AnimState.Happy)
                return;
            animState = AnimState.Happy;
            SkeletonAnim.AnimationState.SetAnimation(0, happyAnim, false);
            PlaySound(animState);
            if (!false)
            {
                tweenDelay?.Kill();
                tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.TakeAPhoto), () =>
                {
                    if (isSit) { PlaySit(); }
                    else { PlayIdle(); }
                });
            }
        }
        public void PlaySad()
        {
            if (sadAnim == null) return;
            if (animState == AnimState.Sad)
                return;
            animState = AnimState.Sad;
            SkeletonAnim.AnimationState.SetAnimation(0, sadAnim, false);
            PlaySound(animState);
        }
        public void PlayLaugh()
        {
            if (laughAnim == null) return;
            if (animState == AnimState.Laugh)
                return;
            animState = AnimState.Laugh;
            SkeletonAnim.AnimationState.SetAnimation(0, laughAnim, false);
            PlaySound(animState);

            tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                if (isSit)
                {
                    PlaySit();
                    return;
                }
                PlayIdle();
            });
        }
        public void PlaySpecial()
        {
            if (specialAnim == null) return;
            if (animState == AnimState.Special)
                return;
            animState = AnimState.Special;
            SkeletonAnim.AnimationState.SetAnimation(0, specialAnim, false);
            PlaySound(animState);

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
            {
                if (isSit)
                {
                    PlaySit();
                    return;
                }
                PlayIdle();
            });
        }
        public void PlayWaveHand(bool isLoop = false)
        {
            if (wavehandAnim == null) return;
            if (animState == AnimState.WaveHand) return;
            animState = AnimState.WaveHand;
            SkeletonAnim.AnimationState.SetAnimation(0, wavehandAnim, isLoop);
            PlaySound(animState);

            if (!isLoop)
            {
                if (tweenDelay != null) tweenDelay?.Kill();
                tweenDelay = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
                {
                    if (isSit)
                    {
                        PlaySit();
                        return;
                    }
                    PlayIdle();
                });
            }
        }
        public void PlayWalk()
        {
            if (walkAnim == null) return;
            if (animState == AnimState.Walk) return;
            animState = AnimState.Walk;
            SkeletonAnim.AnimationState.SetAnimation(0, walkAnim, true);
            PlaySound(animState);
        }

        public float GetTimeAnimation(AnimState animState)
        {
            var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(idleAnim);
            switch (animState)
            {
                case AnimState.Sad:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(sadAnim);
                    break;
                case AnimState.Special:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(specialAnim);
                    break;
                case AnimState.WaveHand:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(wavehandAnim);
                    break;
                case AnimState.Laugh:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(laughAnim);
                    break;
                case AnimState.Walk:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(walkAnim);
                    break;
                case AnimState.Eat:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(EatAnim);
                    break;
                case AnimState.Dizzy:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(dizzyAnim);
                    break;
                case AnimState.Sit:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(sitAnim);
                    break;
                case AnimState.TakeAPhoto:
                    myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(takeAPhoto);
                    break;
            }

            float animLength = myAnimation.Duration;
            return animLength;
        }

        public enum AnimState
        {
            None,
            Idle,
            Run,
            Jump,
            Happy,
            Sad,
            Special,
            WaveHand,
            Laugh,
            Walk,
            Eat,
            Disagree,
            Dizzy,
            Sit,
            TakeAPhoto,
            Sleep
        }
        #endregion
    }
}
