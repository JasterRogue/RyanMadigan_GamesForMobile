using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    private string gameId = "4054850";
#elif UNITY_ANDROID
    private string gameId = "4054851";
#endif

    Button myButton;
    private string mySurfacingId = "rewardVideo1";

    void Start()
    {
        myButton = GetComponent<Button>();
        StartCoroutine(showVideoWhenReady());

        // Set interactivity to be dependent on the Ad Unit or legacy Placement’s status:
        myButton.interactable = Advertisement.IsReady(mySurfacingId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);

       
        Advertisement.Initialize(gameId, true);
        ShowAd("");
       
    }

    void Update()
    {

        //myButton.interactable = Advertisement.IsReady(mySurfacingId);
        if(mySurfacingId == "rewardVideo1")
        {
            ShowAd("");
            Debug.Log("Show reward ad");
        }
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {   
        Advertisement.Show(mySurfacingId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, activate the button: 
        if (surfacingId == mySurfacingId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("“The ad did not finish due to an error.”");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    IEnumerator showVideoWhenReady()
    {
        Debug.Log("Waiting");

        while(!Advertisement.IsReady(mySurfacingId))
            yield return null;

        print("Showing");
        

        Advertisement.Show(mySurfacingId);

    }//end of IENumerator

    public void ShowAd(string zone = "")
    {
        StartCoroutine(showVideoWhenReady());
    }
}