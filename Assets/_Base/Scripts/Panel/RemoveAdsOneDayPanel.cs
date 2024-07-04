using _Base;
using _WolfooCity;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using SCN.IAP;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class RemoveAdsOneDayPanel : UIPanel
    {
        [SerializeField] Button confirmBtn;
        [SerializeField] Button xBtn;
        [SerializeField] TMP_Text adsWatchedTxt;
        [SerializeField] ParticleSystem lightingFx;
        [SerializeField] ParticleSystem smokeFx;
        [SerializeField] ParticleSystem[] confettiFxs;

        int totalAds = 3;
        int countAds;

        public static Action OnOpenPremium;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenText;

        private void Start()
        {
            xBtn.onClick.AddListener(() =>
            {
                Hide(() =>
                {
                    gameObject.SetActive(false);
                });
            });

            confirmBtn.onClick.AddListener(OnConfirm);
            countAds = BaseDataManager.Instance.CountAdsSuccess;
            adsWatchedTxt.text = $"{countAds}";
        }

        private void OnWatchAdsSuccess()
        {
            countAds++;
            OnChangeText();
        }

        private void OnChangeText()
        {
            _tweenText = adsWatchedTxt.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                adsWatchedTxt.text = $"{countAds}";
                _tweenText = adsWatchedTxt.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    if (countAds >= totalAds)
                    {
                        lightingFx.Play();
                        smokeFx.Play();
                        confettiFxs[0].Play();
                        confettiFxs[1].Play();
                        confirmBtn.gameObject.SetActive(false);
                        Invoke("Close", 3);
                    }
                });
            });
        }

        private void Close()
        {
            Hide(() =>
            {
                gameObject.SetActive(false);
                OnOpenPremium?.Invoke();
            });
        }

        private void OnConfirm()
        {
            if (AdsManager.Instance.HasRewardVideo)
            {
                AdsManager.Instance.ShowRewardVideo(() =>
                {
                    OnWatchAdsSuccess();
                    _Base.GameController.Instance.OnWatchAdsSuccess();
                });
            }
        }
    }
}
