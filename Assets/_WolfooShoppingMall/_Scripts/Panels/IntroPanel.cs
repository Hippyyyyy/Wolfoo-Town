using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SCN.IAP;
//using SCN.Ads;
using SCN;

namespace _WolfooShoppingMall
{
    public class IntroPanel : Panel
    {
        [SerializeField] Button playBtn;
        [SerializeField] Button iapBtn;
        [SerializeField] Button settingBtn;
        [SerializeField] Image logoImg;
        [SerializeField] Transform[] moveBtnZones;
        [SerializeField] Transform backGround;

        private Vector3 startScale;
        List<Vector3> listEndTrans = new List<Vector3>();
        private Tweener loopScaleTween;
        private Tweener moveTween;
        private Tweener scaleLogoTween;
        private Tweener rotateTween;
        private Vector3 startWolfooScale;
        private Vector3 startBtnScale;

        protected override void Awake()
        {
            startScale = logoImg.transform.localScale;
            logoImg.transform.localScale = Vector3.zero;

            startBtnScale = playBtn.transform.localScale;
            playBtn.transform.position = moveBtnZones[0].position;
        }
        private void OnEnable()
        {
        //    iapBtn.gameObject.SetActive(!AdsManager.Instance.IsRemovedAds);
        }

        protected override void Start()
        {
            playBtn.onClick.AddListener(OnPlaygame);
            iapBtn.onClick.AddListener(OnOpenIapPanel);
            settingBtn.onClick.AddListener(OnClickSetting);

            EventDispatcher.Instance.RegisterListener<EventKey.OnRemoveAds>(GetBuyRemoveAds);
     //       AdsManager.Instance.HideBanner();

            // Logo Image
            scaleLogoTween = logoImg.transform.DOScale(startScale, 1).SetEase(Ease.OutBack).OnComplete(() =>
            {
                // Button
                moveTween = playBtn.transform.DOMoveY(moveBtnZones[1].position.y, 1).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    loopScaleTween = playBtn.transform.DOPunchScale(Vector3.one * 0.1f, 1, 1).SetLoops(-1, LoopType.Yoyo);
                });
            });

            if (Camera.main.aspect >= 1.5)
            {
                backGround.transform.localScale = transform.localScale - Vector3.one * 0.1f;
            }
        }


        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnRemoveAds>(GetBuyRemoveAds);

            rotateTween?.Kill();
            scaleLogoTween?.Kill();
            moveTween?.Kill();
            loopScaleTween?.Kill();
        }

        void GetBuyRemoveAds()
        {
            //  FirebaseManager.instance.LogBuyIAP("");
            iapBtn.gameObject.SetActive(false);
        }

        private void OnOpenIapPanel()
        {
            StartCoroutine(IAPManager.Instance.IEStart());
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            //IAPPolicyDialog.Instance.OpenDialog(() =>
            //{
            //    //GUIManager.instance.OpenPanel(PanelType.IAP);
            //    IAPManager.Instance.OpenPanel();
            //});
        }
        private void OnClickSetting()
        {
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            GUIManager.instance.OpenPanel(PanelType.Setting);
        }

        private void OnPlaygame()
        {
            playBtn.interactable = false;
            loopScaleTween?.Kill();

            playBtn.transform.localScale = startBtnScale;
            loopScaleTween = playBtn.transform.DOPunchScale(Vector3.one * -0.01f, 0.5f, 1);
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            gameObject.SetActive(false);
            playBtn.interactable = true;
        }
    }
}