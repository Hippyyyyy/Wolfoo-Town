using SCN.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public class NativeAdvance : MonoBehaviour
{
    [SerializeField] RawImage AdIconTexture;
    [SerializeField] Text AdHeadline;
    [SerializeField] Text AdDescription;
    [SerializeField] GameObject AdLoaded;
    [SerializeField] GameObject AdLoading;

    private NativeAd nativeAd;
    private bool nativeLoaded = false;

    private void OnEnable()
    {
        AdLoaded.gameObject.SetActive(false);
        AdLoading.gameObject.SetActive(true);
        RequestNativeAd();
    }
    #region NativeAds
    private void RequestNativeAd()
    {
        AdmobConfig instance = AdmobConfig.Instance;
        AdLoader adLoader = new AdLoader.Builder(instance.NativeID)
            .ForNativeAd()
            .Build();
        adLoader.OnNativeAdClicked += HandleCustomNativeAdClicked;
        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        adLoader.LoadAd(new AdRequest.Builder().Build());

    }

    private void HandleCustomNativeAdClicked(object sender, EventArgs e)
    {
        Debug.Log("Custom Native ad asset with name " + sender + " was clicked. Args: " + e);
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
    }


    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        Debug.Log("Native ad loaded.");
        this.nativeAd = args.nativeAd;

        //register gameobjects with native ads api
        if (!nativeAd.RegisterIconImageGameObject(AdIconTexture.gameObject))
        {
            Debug.Log("error registering icon");
        } else
        {
            AdIconTexture.texture = nativeAd.GetIconTexture();
        }
        if (!nativeAd.RegisterHeadlineTextGameObject(AdHeadline.gameObject))
        {
            Debug.Log("error registering headline");
        } else
        {
            AdHeadline.text = nativeAd.GetHeadlineText();
        }
        if (!nativeAd.RegisterBodyTextGameObject(AdDescription.gameObject))
        {
            Debug.Log("error registering description");
        } else
        {
            AdDescription.text = nativeAd.GetBodyText();
        }

        //disable loading and enable ad object
        AdLoaded.gameObject.SetActive(true);
        AdLoading.gameObject.SetActive(false);
    }
    #endregion
}
