using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Helper;
using Spine.Unity;
using SCN.Common;

public class LoadingAnimation : MonoBehaviour
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
    private RandomNoRepeat<string> rdSkinNrp;

    public enum Type
    {
        Corn,
        Flour,
        Gift,
        Lion,
        Monkey,
        Durian
    }

    private void Start()
    {
        rdSkinNrp = new RandomNoRepeat<string>(skinList);
    }

    public void ChangeSkin(Type colorType)
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
        skeletonAnim.Skeleton.SetSkin(rdSkinNrp.Random());
        skeletonAnim.Skeleton.SetSlotsToSetupPose();
    }

    #region Anim by Spine
    public void PlayExcute()
    {
        if (animState == AnimState.Excute) return;
        animState = AnimState.Excute;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, excuteAnim, true);
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
