using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class AlertButton : BackItem
    {
        [SerializeField] Image actionImg;
        private Tweener fadeTween;
        private Tween _delayTween;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;
        }
        private void OnDestroy()
        {
            if (_delayTween != null) _delayTween?.Kill();
            if (fadeTween != null) fadeTween?.Kill();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            SoundManager.instance.PlayOtherSfx(myClip);
        }
        public void OnPressDanger(float actionTime)
        {
            _delayTween?.Kill();
            fadeTween = actionImg.DOFade(0.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            _delayTween = DOVirtual.DelayedCall(actionTime, () =>
            {
                fadeTween?.Kill();
            });
        }
        public void OnPressLighting()
        {

        }
    }
}