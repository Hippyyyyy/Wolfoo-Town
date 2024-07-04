using _Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooCity
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] PanelType panelType;
        [SerializeField] Image background;
        [SerializeField] Ease ease = Ease.OutBack;

        private Vector3 startScale;
        bool isStarted = true;
        private Tweener scaleBackTween;
        public static Action OnPanelShow;
        public static Action OnShowAdsComplete;

        public PanelType PanelType { get => panelType; }

        private void Awake()
        {
            startScale = background.transform.localScale;
            background.transform.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            transform.SetAsLastSibling();

            if (scaleBackTween != null) scaleBackTween?.Kill();
            scaleBackTween = background.transform.DOScale(startScale, 0.5f)
            .SetEase(ease)
            .OnComplete(() =>
            {
                OnPanelShow?.Invoke();
            });

            if (isStarted)
            {
                isStarted = false;
            }
        }

        public void Hide(System.Action OnComplete)
        {
            OnShowAdsComplete = OnComplete;
            if(AdsManager.Instance.HasInters)
            {
                AdsManager.Instance.ShowInterstitial(() =>
                {
                    OnHide();
                });
            }
            else
            {
                OnHide();
            }
        }
        void OnHide()
        {
            if (scaleBackTween != null) scaleBackTween?.Kill();
            scaleBackTween = background.transform.DOScale(.25f, 0.25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                OnShowAdsComplete?.Invoke();
            });
        }
        public void Show(System.Action OnShowCompleted = null)
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
            if (scaleBackTween != null) scaleBackTween?.Kill();
            scaleBackTween = background.transform.DOScale(1, 0.25f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                OnShowCompleted?.Invoke();
            });
        }
    }
}