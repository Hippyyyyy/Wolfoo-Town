using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Helper;
using Spine.Unity;
public class HouseAnimation : MonoBehaviour
{
    [Header("<======== SPINE =======>")]
    [SerializeField] private SkeletonGraphic skeletonAnim;
    [Header("Idle")]
    [SerializeField, SpineAnimation] private string idleAnim;
    [Header("IdleExcuted")]
    [SerializeField, SpineAnimation] private string idleExcutedAnim;
    [Header("Excute")]
    [SerializeField, SpineAnimation] private string excuteAnim;

    private AnimState animState;
    public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

    [Header("Skin")]
    [SerializeField, SpineSkin] string[] skinList;
    private Tween _tween;

    public enum ColorType
    {
        Red,
        Chocho,
        Yellow,
        Green,
        Blue,
    }

    public void ChangeSkin(ColorType colorType)
    {
        skeletonAnim.Skeleton.SetSkin(skinList[(int)colorType]);
        skeletonAnim.Skeleton.SetSlotsToSetupPose();
    }
    public void ChangeSkin(int idx)
    {
        skeletonAnim.Skeleton.SetSkin(skinList[idx]);
        skeletonAnim.Skeleton.SetSlotsToSetupPose();
    }
    public void ChangeSkin()
    {
        skeletonAnim.Skeleton.SetSkin(skinList[Random.Range(0, skinList.Length)]);
        skeletonAnim.Skeleton.SetSlotsToSetupPose();
    }

    private void OnDestroy()
    {
        if (_tween != null) _tween?.Kill();
    }

    #region Anim by Spine
    public void PlayExcute(System.Action OnCompleted = null)
    {
        if (animState == AnimState.Excute) return;
        animState = AnimState.Excute;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, excuteAnim, false);

        _tween?.Kill();
        _tween = DOVirtual.DelayedCall(GetTimeAnimation(animState), () =>
        {
            OnCompleted?.Invoke();
        });
    }
    public void PlayIdle()
    {
        if (animState == AnimState.Idle)
            return;
        animState = AnimState.Idle;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, idleAnim, true);
    }
    public void PlayIdleExcuted()
    {
        if (animState == AnimState.IdleExcuted)
            return;
        animState = AnimState.IdleExcuted;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, idleExcutedAnim, true);
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
            case AnimState.IdleExcuted:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(idleExcutedAnim);
                break;
        }

        float animLength = myAnimation.Duration;
        return animLength;
    }

    public enum AnimState
    {
        None,
        Idle,
        IdleExcuted,
        Excute,
    }
    #endregion

}
