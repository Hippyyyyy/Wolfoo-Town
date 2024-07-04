using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace _WolfooShoppingMall
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] Image loadingImg;
        [SerializeField] Image backgroundImg;
        [SerializeField] LoadingAnimation loadingAnimation;
        [SerializeField] float timeLoading;
        [SerializeField] float timeChanging;

        public UnityEvent OnLoadComplete;

        private Tweener fadeTween;
        private Tween delayTween;
        private Tween _animTween;
        private Tweener floatTween;
        private bool isChanging;
        private bool isClosing;

        private void Awake()
        {
            backgroundImg.DOFade(0, 0);
        }
        private void OnDestroy()
        {
            if (fadeTween != null) fadeTween?.Kill();
            if (delayTween != null) delayTween?.Kill();
            if (_animTween != null) _animTween?.Kill();
            if (floatTween != null) floatTween?.Kill();
        }

        public void Close(float _delay, System.Action OnComplete = null)
        {
            if (isClosing) return;
            isClosing = true;

            if (delayTween != null) delayTween?.Kill();
            fadeTween?.Kill();
            backgroundImg.DOFade(1, 0);
            delayTween = DOVirtual.DelayedCall(_delay, () =>
            {
                fadeTween = backgroundImg.DOFade(0, 0.2f).OnComplete(() =>
                {
                //    fadeTween = loadingImg.DOFade(startFadeLoading, 0);
                    gameObject.SetActive(false);
                    OnComplete?.Invoke();
                });
            });
        }

        public void Open(bool isLoop)
        {
            isClosing = false;
            fadeTween = backgroundImg.DOFade(1, 0.2f).OnComplete(() =>
            {
                floatTween = DOVirtual.Float(0, timeLoading, timeLoading, (progress) =>
                {
                    if (isChanging) return;

                    isChanging = true;
                    _animTween = DOVirtual.DelayedCall(timeChanging, () =>
                    {
                        isChanging = false;
                        loadingAnimation.ChangeSkin();
                    });
                }).SetLoops(isLoop ? -1 : 0, LoopType.Restart);
            });
        }

        public void Open(System.Action OnLoading = null, System.Action OnComplete = null)
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            isClosing = false;
            fadeTween = backgroundImg.DOFade(1, 0.2f).OnComplete(() =>
            {
                OnLoading?.Invoke();
                floatTween = DOVirtual.Float(0, timeLoading, timeLoading, (progress) =>
                {
                    if (isChanging) return;

                    isChanging = true;
                    _animTween = DOVirtual.DelayedCall(timeChanging, () =>
                    {
                        isChanging = false;
                        loadingAnimation.ChangeSkin();
                    });
                })
                .OnComplete(() =>
                {
                    Close(0, OnComplete);
                });
            });

        }
    }
}