using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{
    private BannerView bannerView;

    [Header("Ads IDs")]
    public string AndroidBannerId;
    public string IPhoneBannerId;
    // Start is called before the first frame update
    void Start()
    {
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

        bannerView = new BannerView(bannerId, AdSize.IABBanner, AdPosition.Top);
    
        AdRequest request = new AdRequest.Builder().Build();

        bannerView.LoadAd(request);
    }

    private void OnDestroy()
    {
        bannerView.Destroy();
    }
}
