using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _WolfooSchool
{
    public class IAPPanel : Panel
    {
        [SerializeField] Button tryFreeBtn;
        [SerializeField] Button buyBtn;
        [SerializeField] Button backBtn;

        protected override void Awake()
        {
            EventManager.OnShowPanel += InitScreen;
        }
        private void OnDestroy()
        {
            EventManager.OnShowPanel += InitScreen;
        }

        private void InitScreen()
        {

            if (DataSceneManager.Instance.LocalDataStorage.isRemoveAds)
            {
                buyBtn.interactable = false;
            }
            if (DataSceneManager.Instance.LocalDataStorage.isTryFree)
            {
                tryFreeBtn.interactable = false;
            }
        }

        protected override void Start()
        {
            tryFreeBtn.onClick.AddListener(OnTryFreeClick);
            buyBtn.onClick.AddListener(OnBuyClick);
            backBtn.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnTryFreeClick()
        {
            DataSceneManager.Instance.LocalDataStorage.isTryFree = true;
        }

        private void OnBuyClick()
        {
            DataSceneManager.Instance.LocalDataStorage.isRemoveAds = true;
        //    AdsManager.Instance.SetRemovedAds();
            buyBtn.interactable = false;
            buyBtn.transform.DOPunchScale(Vector3.one * .1f, 0.5f, 5);
        }
    }
}