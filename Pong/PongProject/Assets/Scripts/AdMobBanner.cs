using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdMobBanner : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    ComputerControl myComputerControl;

    int whichBanner;
    int whichInterstetial;
    int whichRewardVideo;

    //unity ads 
    string gameId = "4054850";
    bool testMode = true;
    string bannerIDUnity = "Banner_Android";
    string vidIdUnity = "Rewarded_Android";
    public Button myButton;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        Advertisement.Initialize(gameId, testMode);
        //myButton = GetComponent<Button>();

        myComputerControl = GameObject.Find("Paddle2").GetComponent<ComputerControl>();

        whichBanner = Random.Range(1, 3);
       
        whichBannerToShow();
    }  

    public void whichBannerToShow()
    {
        print("Banner num "  + whichBanner);

        if(whichBanner == 1)
        {
            //google banner
            this.requestBannerGoogle();
        }

        else
        {
            //unity banner
            StartCoroutine(showUnityBanner());
        }
        
    }//end of which banner to show

    public void whichInterstetialToShow()
    {

        print("interstetial val: " + whichInterstetial);

        whichInterstetial = Random.Range(1, 3);

        if (whichInterstetial == 1)
        {
            //show google insterstetial
            this.requestInterstetialGoogle();
        }

        else
        {
            //show unity interstetial
            ShowInterstitialAdUnity();
        }

    }//end of which interstetial to show

    public void whichRewardVidToShow()
    {
        whichRewardVideo = Random.Range(1, 3);

        if (whichRewardVideo == 1)
        {
            //show google vid
            requestRewardAdGoogle();
        }

        else
        {
            //show unity vid
            UnityShowVidAd("");
        }

    }//end of which reward ad to show

    public void requestInterstetialGoogle()
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
            this.interstitial.Show();
            //print("Interstetial ad showing");
        }
    }

    private void requestBannerGoogle()
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

    public void requestRewardAdGoogle()
    {
        string rewardVidId = "ca-app-pub-6481652980135809/1798854520";

        this.rewardedAd = new RewardedAd(rewardVidId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            //print("Reward Ad Google Show ");
        }
    }//end of requestRewardAdGoogle

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        myComputerControl.lowerCPUSpeed();
    }

    IEnumerator showUnityBanner()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);
        Advertisement.Banner.Show(bannerIDUnity);
        
    }

    public void ShowInterstitialAdUnity()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }//end of interstitialad

    IEnumerator showVideoWhenReadyUnity()
    {
        Debug.Log("Waiting");

        while (!Advertisement.IsReady(vidIdUnity))
            yield return null;

        print("Showing");


        Advertisement.Show(vidIdUnity);

    }//end of IENumerator

    public void UnityShowVidAd(string zone = "")
    {
        if (Advertisement.IsReady(vidIdUnity))
        {
            //countdownTime = 5f;
            Advertisement.Show(vidIdUnity);
            Debug.Log("Unity Rewarded");
        }

        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            requestRewardAdGoogle();
        }
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            myComputerControl.lowerCPUSpeed();
            print("Reward vid watched successfully");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            print("User skipped ad. No reward for you.");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("“The ad did not finish due to an error.”");
        }
    }//OnUnityAdsDidFinish() ends

    public void OnUnityAdsReady()
    {
        myButton.interactable = true;
    }
}
