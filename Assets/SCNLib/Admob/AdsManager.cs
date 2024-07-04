using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SCN.Ads;
using SCN.IAP;
using _Base;
using SCN;
using _WolfooShoppingMall;

public class AdsManager : MonoBehaviour
{
    private float interval = 40;
    private float lastTimeInter;
    private int countInter=0;

    private static AdsManager _instance;
    public static AdsManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = (new GameObject("AdsManager")).AddComponent<AdsManager>();
            return _instance;
        }
    }

    public static bool Initialized => _instance != null;

    /// <summary>
    /// Use to listening when the reward video ad is available, usefull for turn on some ad button.
    /// </summary>
    public event Action onRewardAvailable;

    [SerializeField] private bool useAdmob = true;
    private bool useAdUnityBackup;

    #region Remove Ads
    private const string RemoveAdsKey = "NO_ADS";
    /// <summary>
    /// If true, User no longer to see Banner or Interstitial ads, but still can see Reward video ads.
    /// </summary>
    public bool IsRemovedAds
    {
        get => PlayerPrefs.HasKey(RemoveAdsKey) || PlayerPrefs.GetInt("SUBSCRIBED") == 1 || _Base.GameController.Instance.HasPremiumDay;
    }

    /// <summary>
    /// Remove all Banner & Interstital ads (Still keep reward video ads)
    /// <para>Call this when user buy Remove_Ads.</para>
    /// </summary>
    public void SetRemovedAds()
    {
        PlayerPrefs.SetInt(RemoveAdsKey, 1);
        DestroyBanner();
    }
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (useAdmob)
        {
            Debug.Log($"[Ads] Initializing... useUnity={useAdUnityBackup}, log={AdmobConfig.Instance.EnableLog}, test={AdmobConfig.Instance.UseTestID}");
            gameObject.AddComponent<AdsAdmob>();
            AdsAdmob.Instance.OnRewardAdLoaded += OnRewardAvailable;
        }
    }

    private void OnRewardAvailable()
    {
        onRewardAvailable?.Invoke();
    }

    /// <summary>
    /// Call this to preload ads to show ingame. Should be call at the first application script
    /// </summary>
    public void Preload() { }

    public void Preload(Transform parent)
    {
        transform.SetParent(parent);
    }


    public float countPlay = 0;
    public bool HasInters
    {
        get
        {
            var isUnlockAll = false;
            if (_WolfooSchool.DataSceneManager.Instance != null)
            {
                isUnlockAll = _WolfooSchool.DataSceneManager.Instance.IsForceUnlockAll;
            }
            if (_WolfooShoppingMall.DataSceneManager.Instance != null)
            {
                isUnlockAll = _WolfooShoppingMall.DataSceneManager.Instance.IsForceUnlockAll;
            }
            if (isUnlockAll || IsRemovedAds || Time.time <= lastTimeInter||IAPManager.Instance.IsSubscribed /*|| countInter >=3*/) return false;
            return (useAdmob && AdsAdmob.Instance.HasInter);
        }
    }

    public bool HasRewardVideo
    {
        get
        {
            return (useAdmob && AdsAdmob.Instance.HasRewardVideo);
        }
    }
 

    public void ShowBanner()
    {
        Debug.Log("Show Banner.....");
        if (!IsRemovedAds && useAdmob)
        {
            AdsAdmob.Instance.ShowBanner();
        }
    }

    public void HideBanner()
    {
        Debug.Log("Hide Banner.....");
        if (!IsRemovedAds && useAdmob)
        {
            AdsAdmob.Instance.HideBanner();
        }
    }

    public void DestroyBanner()
    {
        if (useAdmob)
        {
            AdsAdmob.Instance.DestroyBanner();
        }
    }

    public float GetBannerHeight()
    {
        if (!IsRemovedAds && useAdmob) return AdsAdmob.Instance.BannerHeight;
        return 0;
    }

    public void ShowInterstitial(Action callback = null)
    {
        if (IsRemovedAds)
        {
            lastTimeInter = Time.time + interval;
            callback?.Invoke();
            return;
        }

        OnShowAdStart();
        callback += OnShowAdFinished;

        if (HasInters)
        {
            countInter += 1;
            countPlay = 0;
            lastTimeInter = Time.time + interval;
            AdsAdmob.Instance.ShowInterstitial(() =>
            {
                callback?.Invoke();
                EventDispatcher.Instance.Dispatch(new EventKey.OnWatchInterAds());
            });
        }
        else callback?.Invoke();
    }

    public void ShowRewardVideo(Action onSuccess, Action onClosed = null)
    {
        OnShowAdStart();
        onSuccess += OnShowAdFinished;
        onClosed += OnShowAdFinished;

        if (HasRewardVideo)
        {
            lastTimeInter = Time.time + interval;
            AdsAdmob.Instance.ShowRewardVideo(onSuccess, onClosed);
        }
        else onClosed?.Invoke();
    }

    private void OnShowAdStart()
    {
    }

    private void OnShowAdFinished()
    {
    }

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (isAdShowing)
    //    {
    //        Debug.Log($"OnApplicationFocus: focus={focus}, isAdShowing={isAdShowing}");
    //        Application.runInBackground = !focus;
    //    }
    //}

}
