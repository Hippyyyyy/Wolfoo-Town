using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Fertilizer : BackItem
    {
        [SerializeField] ParticleSystem nutsFx;
        [SerializeField] Animator animator;
        [SerializeField] string playName;

        protected Sprite[] myData;

        public Action<Sprite> OnCompleted { get; private set; }

        protected override void Start()
        {
            animator.enabled = false;
            base.Start();
        }

        public void OnPouringCompleted()
        {
            nutsFx.Stop();
            animator.enabled = false;
            transform.SetParent(Content.transform);
            OnCompleted?.Invoke(myData[UnityEngine.Random.Range(0, myData.Length)]);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (animator != null) animator.enabled = false;
        }
        public void Pouring(Vector3 endPos, System.Action<Sprite> OnCompleted)
        {
            IsAssigned = true;
            KillDragging();

            tweenMove = transform.DOMove(endPos, 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.enabled = true;
                animator.Play(playName, 0, 0);
                this.OnCompleted = OnCompleted;
            });

        }
    }
}
