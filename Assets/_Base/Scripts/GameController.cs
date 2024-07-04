using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Base
{
    public class GameController : SingletonBindAlive<GameController>
    {
        public bool HasPremiumDay;
        public bool IsTrialPremium;

#if UNITY_EDITOR
        public bool IsTestingMap;
        public bool IsTestingData;
        [NaughtyAttributes.ShowIf("IsTestingMap")]
        public AssetLabelReference roomsLabel;
#endif

        private void Start()
        {
            //   EventDispatcher.Instance.RegisterListener<_WolfooShoppingMall.EventKey.OnWatchInterAds>(OnOtherWatchAds);
            //   EventDispatcher.Instance.RegisterListener<_WolfooSchool.EventKey.OnWatchAds>(OnSchoolWatchAds);
            //       EventDispatcher.Instance.RegisterListener<_WolfooShoppingMall.EventKey.OnWatchAds>(OnOtherWatchAds);

#if UNITY_EDITOR
            if (IsTestingMap)
            {
                Invoke("CreateTestingMap", 1);
            }
#endif
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //      EventDispatcher.Instance.RemoveListener<_WolfooSchool.EventKey.OnWatchAds>(OnSchoolWatchAds);
            //      EventDispatcher.Instance.RemoveListener<_WolfooShoppingMall.EventKey.OnWatchAds>(OnOtherWatchAds);
         //   EventDispatcher.Instance.RemoveListener<_WolfooShoppingMall.EventKey.OnWatchInterAds>(OnOtherWatchAds);
        }

#if UNITY_EDITOR
        private void CreateTestingMap()
        {
            _WolfooShoppingMall.DataSceneManager.Instance.LoadRoomData(roomsLabel, () =>
            {
                LoadSceneManager.Instance.OnLoadCompleted?.Invoke();
            });
        }
#endif

        private void OnOtherWatchAds()
        {
            OnWatchAdsSuccess();
        }

        public void CheckPremiumDay()
        {
            HasPremiumDay = BaseDataManager.Instance.CountAdsSuccess >= 3 && IsTrialPremium;
        }
        public void OnWatchAdsSuccess()
        {
            Debug.Log("OnWatchAds++");
            BaseDataManager.Instance.CountAdsSuccess++;
            var countWatchAds = BaseDataManager.Instance.CountAdsSuccess;
            if (countWatchAds >= 3)
            {
                // Show Panel
                // Unlock Content
                HasPremiumDay = true && IsTrialPremium;
            }
        }


    }
}
