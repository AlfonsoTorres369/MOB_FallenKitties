using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager Instance
    {
        get;
        private set;
    }

    //Ads references
    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    [Header("Ads IDs")]
    public string AndroidBannerId;
    public string IPhoneBannerId;
    public string AndroidInterId;
    public string IPhoneInterId;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    void Start()
    {
        //Initializing Ads system
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string bannerId = AndroidBannerId;
        #elif UNITY_IPHONE
            string bannerID = IPhoneBannerId;
        #else
            string bannerId = "unexpected_platform";
        #endif

        // Initialize a BannerView
        bannerView = new BannerView(bannerId, AdSize.IABBanner, AdPosition.Top);
        
        // Create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        //Load the banner with the request
        bannerView.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string interId = AndroidInterId;
        #elif UNITY_IPHONE
            string interId = IPhoneInterId;
        #else
            string interId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        interstitialAd = new InterstitialAd(interId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        
        // Load the interstitial with the request.
        interstitialAd.LoadAd(request);

        interstitialAd.OnAdClosed += InterstitialAdClosed;

        // Showing the interstitial ad
        if(interstitialAd.IsLoaded())
            interstitialAd.Show();
    }

    private void InterstitialAdClosed(object _sender, EventArgs _args)
    {
        if(interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
    }

    private void OnDestroy()
    {
        if(bannerView != null)
            bannerView.Destroy();
    }
}
