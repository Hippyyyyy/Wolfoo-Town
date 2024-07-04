using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Helper;
using Spine.Unity;
public class BreadMachineAnimation : MonoBehaviour
{
    [Header("<======== SPINE =======>")]
    [SerializeField] private SkeletonGraphic skeletonAnim;
    [Header("Idle")]
    [SerializeField, SpineAnimation] private string idleAnim;
    [Header("Excute")]
    [SerializeField, SpineAnimation] private string excuteAnim;
    [Header("Excute2")]
    [SerializeField, SpineAnimation] private string excute2Anim;

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
    public void PlayExcute()
    {
        if (animState == AnimState.Excute) return;
        animState = AnimState.Excute;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, excuteAnim, false);
    }
    public void PlayExcute2()
    {
        if (animState == AnimState.Excute2) return;
        animState = AnimState.Excute2;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, excute2Anim, false);
    }
    public void PlayIdle()
    {
        if (animState == AnimState.Idle)
            return;
        animState = AnimState.Idle;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, idleAnim, true);
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
            case AnimState.Excute2:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(excute2Anim);
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
        Excute2,
    }
    #endregion

}
