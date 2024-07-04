using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Helper;
using Spine.Unity;
public class ElevatorAnimation : MonoBehaviour
{
    [Header("<======== SPINE =======>")]
    [SerializeField] private SkeletonGraphic skeletonAnim;
    [Header("Close")]
    [SerializeField, SpineAnimation] private string closeAnim;
    [Header("Closed")]
    [SerializeField, SpineAnimation] private string closedAnim;
    [Header("Open")]
    [SerializeField, SpineAnimation] private string openAnim;
    [Header("Opened")]
    [SerializeField, SpineAnimation] private string openedAnim;

    private AnimState animState;
    public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

    [Header("Skin")]
    [SerializeField, SpineSkin] string[] skinList;
    private Tween delayTween;
    private Tween delayTween2;

    private void OnDestroy()
    {
        if (delayTween != null) delayTween?.Kill();
        if (delayTween2 != null) delayTween2?.Kill();
    }

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

    public void PlayOpenAnim(System.Action OnComplete = null, System.Action OnOpening = null)
    {
        PlayOpen();
        if (delayTween != null) delayTween?.Kill();
        delayTween = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.Open), () =>
        {
            PlayOpened();
            OnComplete?.Invoke();
        });

        if (delayTween2 != null) delayTween2?.Kill();
        delayTween2 = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.Open) - 1f, () =>
        {
            OnOpening?.Invoke();
        });
    }
    public void PlayCloseAnim(System.Action OnComplete = null, System.Action OnClosing = null)
    {
        PlayClose();
        if (delayTween != null) delayTween?.Kill();
        delayTween = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.Close), () =>
        {
            PlayClosed();
            OnComplete?.Invoke();
        });

        if (delayTween2 != null) delayTween2?.Kill();
        delayTween2 = DOVirtual.DelayedCall(GetTimeAnimation(AnimState.Close) - 1f, () =>
        {
            OnClosing?.Invoke();
        });
    }

    #region Anim by Spine
    public void PlayOpen()
    {
        if (animState == AnimState.Open) return;
        animState = AnimState.Open;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, openAnim, false);
    }
    public void PlayOpened()
    {
        if (animState == AnimState.Opened) return;
        animState = AnimState.Opened;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, openedAnim, true);
    }
    public void PlayClose()
    {
        if (animState == AnimState.Close)
            return;
        animState = AnimState.Close;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, closeAnim, false);
    }
    public void PlayClosed()
    {
        if (animState == AnimState.Closed)
            return;
        animState = AnimState.Closed;
        AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, closedAnim, true);
    }

    public float GetTimeAnimation(AnimState animState)
    {
        var myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closeAnim);
        switch (animState)
        {
            case AnimState.Close:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closeAnim);
                break;
            case AnimState.Open:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openAnim);
                break;
            case AnimState.Opened:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(closedAnim);
                break;
            case AnimState.Closed:
                myAnimation = SkeletonAnim.Skeleton.Data.FindAnimation(openedAnim);
                break;
        }

        float animLength = myAnimation.Duration;
        return animLength;
    }

    public enum AnimState
    {
        None,
        Close,
        Open,
        Opened,
        Closed,
    }
    #endregion

}
