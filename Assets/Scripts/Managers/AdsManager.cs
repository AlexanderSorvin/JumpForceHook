using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    protected string appId = "ca-app-pub-9599555211915120~7597633626";
    protected string bannerID = "ca-app-pub-9599555211915120/7409931364";

    private BannerView bannerView;

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    private void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
}
