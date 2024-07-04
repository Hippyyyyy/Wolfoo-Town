using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Flower : BackItem
    {
        [SerializeField] bool isTied;
        [SerializeField] Sprite peanutSprite;
        [SerializeField] Sprite sproutSprite;
        [SerializeField] Animator animator;
        [SerializeField] string growthAnimName;
        [SerializeField] string sproutAnimName;
        [SerializeField] State curState;
        private Sprite growthSprite;

        public bool IsTied { get => isTied; }
        public bool IsGrowth { get => curState == State.Growth; }
        private float totalGrowthTime = 3;
        private float countTime;
        private bool isGrowthing;

        public enum State
        {
            Seed,
            Growth,
            Sprout,
        }

        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            IsCarry = true;
            if (animator != null)
            {
                animator.enabled = false;
                canDrag = curState == State.Growth;
            }

        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, flower = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            if (animator != null)
            {
                animator.enabled = false;
                image.rectTransform.pivot = Vector2.one * 0.5f;
            }
        }

        public void OnJUmpToPot(Vector3 _endPos, Transform _endParent, System.Action OnComplete = null)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();
            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(_endPos, 100, 1, 0.5f)
            .OnComplete(() =>
            {
                IsAssigned = false;
                OnComplete?.Invoke();
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);
            });
        }

        public void AssginToPeanut(Sprite flowerSprite, bool canEat)
        {
            curState = State.Seed;

            image.sprite = peanutSprite;
            image.SetNativeSize();

            animator.enabled = true;
            animator.Play(growthAnimName, 0, 0);

            growthSprite = flowerSprite;
            IsFood = canEat;
        }

        public void OnAnimGrowthCompleted()
        {
            canDrag = curState == State.Growth;
        }

        public void StopGrowthing()
        {
            countTime = 0f;
            isGrowthing = false;
        }

        public void Growth()
        {
            if (curState == State.Growth) return;
            if (isGrowthing) return;

            countTime += Time.deltaTime;


            if ( curState == State.Sprout)
            {
                countTime = 0;
                KillScalling();
                curState = State.Growth;
                animator.enabled = true;
                animator.Play(growthAnimName, 0, 0);

                image.sprite = growthSprite;
                image.SetNativeSize();
            }
            else if ( curState == State.Seed)
            {
                countTime = 0;
                KillScalling();
                curState = State.Sprout;
                animator.enabled = true;
                animator.Play(sproutAnimName, 0, 0);

                image.sprite = sproutSprite;
                image.SetNativeSize();
            }
        }
    }
}