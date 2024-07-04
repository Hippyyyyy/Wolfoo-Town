using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class PlayAnimObject : BackItem
    {
        [SerializeField] NormalCharacterAnimation _spine;
        [SerializeField] Animator _animator;
        [SerializeField] bool isPlayoneTime = true;
        [SerializeField] string _triggerName = "Run";
        [SerializeField] string[] _triggerActions;
        [SerializeField] float timeAnimCompleted = 0.5f;
        private Tween _tween;
        private int state;

        protected override void InitItem()
        {
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            if (myClip != null) SoundManager.instance.PlayOtherSfx(myClip);

            if (isPlayoneTime)
            {
                if (_spine != null) PlaySpine();
                else if (_animator != null) PlayAnimator();
            }
            else
            {
                state = 1 - state;
                if (_animator != null) { _animator.SetTrigger(_triggerActions[state]); }
            }
        }
        void PlaySpine()
        {
            _spine.PlayExcute();
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(_spine.GetTimeAnimation(NormalCharacterAnimation.AnimState.Excute), () =>
            {
                _spine.PlayIdle();
            });
        }
        void PlayAnimator()
        {
            canClick = false;
            _animator.SetTrigger(_triggerName);

            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(timeAnimCompleted, () =>
            {
                canClick = true;
            });
        }
        public void RunCompletedAnim()
        {
        }
    }
}