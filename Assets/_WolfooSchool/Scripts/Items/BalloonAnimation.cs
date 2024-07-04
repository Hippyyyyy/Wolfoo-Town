using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class BalloonAnimation : MonoBehaviour
    {
        [SerializeField] Button btn;
        [SerializeField] AudioSource aus;

        public enum AnimState
        {
            Idle,
            Explose,
        }

        [Header("<======== SPINE =======>")]
        [SerializeField] private SkeletonGraphic skeletonAnim;
        [Header("Idle")]
        [SerializeField, SpineAnimation] private string idleAnim;
        [Header("Run")]
        [SerializeField, SpineAnimation] private string exploseAnim;
        private AnimState animState;
        private float startScaleTime;

        public SkeletonGraphic SkeletonAnim { get => skeletonAnim; set => skeletonAnim = value; }

        private void Start()
        {
            btn.onClick.AddListener(OnClick);
            startScaleTime = skeletonAnim.timeScale;
            aus.volume = SoundManager.instance.Sfx.volume;
        }

        private void OnClick()
        {
            PlayExplose();
        }
        public void PlayIdle()
        {
            if (animState == AnimState.Idle)
                return;
            animState = AnimState.Idle;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, idleAnim, true);
            SkeletonAnim.timeScale = startScaleTime;
        }
        public void PlayExplose()
        {
            if (animState == AnimState.Explose)
                return;
            skeletonAnim.timeScale = 1;
            animState = AnimState.Explose;
            AnimationHelper.PlayAnimation(SkeletonAnim.AnimationState, exploseAnim, false);
            aus.Play();

            var animation = SkeletonAnim.Skeleton.Data.FindAnimation(exploseAnim);
            Invoke("PlayIdle", animation.Duration);
        }
    }
}
