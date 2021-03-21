using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobBanner : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd; 

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();

        //this.RequestInterstitial();

        //string adUnitId = "ca-app-pub-6481652980135809/1974098431";

        string rewardVidId = "ca-app-pub-6481652980135809/1798854520";

        this.rewardedAd = new RewardedAd(rewardVidId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.

        /*this.rewardedAd.LoadAd(request);

        if (this.rewardedAd.IsLoaded())
        {
            //this.rewardedAd.Show();
            print("Reward Ad Google Show ");
        }*/


    }  

    public void RequestInterstitial()
    {
        //print("Interstetial ad");

        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-6481652980135809/8341686922";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        if (this.interstitial.IsLoaded())
        {
            //this.interstitial.Show();
            print("Interstetial ad showing");
        }
    }

    private void RequestBanner()
    {
        print("Banner method runs");

        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-6481652980135809/1974098431";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        bannerView.Show();
    }

    public void requestRewardAd()
    {
        string rewardVidId = "ca-app-pub-6481652980135809/1798854520";

        this.rewardedAd = new RewardedAd(rewardVidId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        if (this.rewardedAd.IsLoaded())
        {
            //this.rewardedAd.Show();
            print("Reward Ad Google Show ");
        }
    }
}
