using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class ADManager : MonoBehaviour, IUnityAdsListener
{

    #if UNITY_IOS
        private string gameId = "4058590";
    #elif UNITY_ANDROID
        private string gameId = "4058591";
    #endif

    public string surfacingId = "BannerAD";

    GameManager GameScript;
    UIManager UIScript;

    void Start()
    {
        Advertisement.Initialize(gameId, true);
        Advertisement.AddListener(this);

        GameScript = GetComponent<GameManager>();
        UIScript = GetComponent<UIManager>();
    }

    public IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(surfacingId);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void playRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo");
        }
        else
        {
            //No ads available pop up
            Debug.Log("ad not ready");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("ADS ARE READY");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("ERROR: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("VIDEO STARTED");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            //Reward player with 2x shards
            Debug.Log("PLAYER IS REWARDED");

            //Disable the button so it can only be claimed once
            UIScript.GOADButton.interactable = false;

            //Then run the double shards function
            GameScript.doubleShards();
        }
    }
}