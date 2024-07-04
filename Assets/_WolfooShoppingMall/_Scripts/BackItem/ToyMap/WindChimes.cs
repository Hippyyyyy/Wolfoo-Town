using _Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class WindChimes : BackItem
    {
        [SerializeField] WindChimesAnimation windChimesAnimation;
        private Tween tweenDelay;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnPLayAnim();
        }

        private void OnPLayAnim()
        {
            windChimesAnimation.PlayIdle();
            windChimesAnimation.PlayExcute();

            SoundManager.instance.PlayOtherSfx(SfxOtherType.WindChime);
            SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Wow);

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(windChimesAnimation.GetTimeAnimation(WindChimesAnimation.AnimState.Excute), () =>
            {
                windChimesAnimation.PlayIdle();
            });
        }
    }
}