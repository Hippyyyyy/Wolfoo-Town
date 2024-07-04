using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ClawRopeMachine : ItemMove
    {
        [SerializeField] Animator animator;
        [SerializeField] float velocity;
        [SerializeField] float speed;
        [SerializeField] float failRate;
        [SerializeField] Transform[] claws;
        [SerializeField] Transform itemZone;
        [SerializeField] Transform limitUnderZone;
        [SerializeField] ParticleSystem failFx;
        [SerializeField] CircleCollider2D motorCollider;

        private bool canMoveDown;
        private Tween delayTween;
        private bool isGrab;
        private Vector3 startPOs;
        private Tweener tweenMove;
        private bool isGrabFail;

        public float Velocity { get => velocity; }
        public Transform ItemZone { get => itemZone; }

        private void Awake()
        {
            _WolfooCity.UIPanel.OnPanelShow += GetModeShow;
        }
        private void OnDestroy()
        {
            if (tweenMove != null) tweenMove?.Kill();
            if (delayTween != null) delayTween?.Kill();
            _WolfooCity.UIPanel.OnPanelShow -= GetModeShow;
        }

        private void GetModeShow()
        {
            startPOs = transform.position;
        }

        public void PLayPickup(System.Action OnComplete = null)
        {
            if (isGrab) return;
            isGrab = true;

            animator.SetTrigger("Grab");
            tweenMove = transform.DOMoveY(limitUnderZone.position.y, speed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    OnComplete?.Invoke();
                });
        }

        public void OnPickup(System.Action OnComplete)
        {
            if (tweenMove != null) tweenMove?.Kill();

            animator.SetTrigger("Grab2");
            delayTween = DOVirtual.DelayedCall(0.5f, () =>
            {
                tweenMove = transform.DOMoveY(startPOs.y, speed)
                .SetSpeedBased(true)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    isGrab = false;
                    OnComplete?.Invoke();
                    motorCollider.enabled = true;
                });
            });
        }
        //public void OnCollideToy(ClawMachineToy toy, Transform _parent, System.Action OnCOmplete, System.Action OnFail)
        //{
        //    if (isGrab) return;
        //    isGrab = true;

        //    animator.SetTrigger("Grab2");
        //    isGrabFail = false;
        //    delayTween = DOVirtual.DelayedCall(.15f, () =>
        //    {
        //        canMoveDown = false;

        //        if (curToy == null)
        //        {
        //            curToy = toy;
        //            curToy.OnGrabbing(_parent);
        //        }

        //        delayTween = DOVirtual.DelayedCall(0.5f, () =>
        //        {
        //            tweenMove = transform.DOMoveY(startPOs.y, speed).SetSpeedBased(true).SetEase(Ease.InBack).OnComplete(() =>
        //            {
        //                isGrab = false;

        //                if(!isGrabFail)
        //                    OnCOmplete?.Invoke();
        //                else 
        //                    OnFail?.Invoke();
        //            });

        //            int rd = Random.Range(0, 100);
        //            if (rd < failRate) // On Fail
        //            {
        //                isGrabFail = true;
        //                OnGrabFail();
        //            }
        //        });
        //    });
        //}

        public void OnGrabFail()
        {
            if (failFx != null)
                failFx.Play();

            motorCollider.enabled = false;
            animator.SetTrigger("Grab");
        }

        public void OnReleaseItemIntoBox(Vector3 _endPos, System.Action OnCOmplete)
        {
            if (tweenMove != null) tweenMove?.Kill();
            tweenMove = transform.DOMoveX(_endPos.x, speed).SetSpeedBased(true)
                .OnComplete(() =>
                {
                    animator.SetTrigger("Grab");
                    OnCOmplete?.Invoke();
                });

        }
    }
}