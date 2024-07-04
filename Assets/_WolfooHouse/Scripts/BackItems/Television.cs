using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Television : BackItem
    {
        [SerializeField] Image introImg;
        [SerializeField] Animator _anim;
        private bool isOpen;
        private Tweener _tween;

        protected override void InitItem()
        {
            canClick = true;
            isOpen = false;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            isOpen = !isOpen;
            OnPlay();
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }

        private void OnPlay()
        {
            _tween = introImg.DOFillAmount(isOpen ? 0 : 1, 0.5f).OnComplete(() =>
            {
                if (isOpen)
                {
                    _anim.SetTrigger("PlayingTV");
                }
                else
                {
                    _anim.SetTrigger("PlayIdleTV");
                }
            });
        }
    }
}