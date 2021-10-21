using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.SocialPlatforms;
using TMPro;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager current;

    [SerializeField] private TMP_Text userScoreText;

	private void Awake()
    {
        current = this;

        if (!RuntimeManager.IsInitialized())
        {
            RuntimeManager.Init();
        }
    }

    private void Start()
    {
        if (!GameServices.IsInitialized())
        {
            GameServices.Init();
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
    }

    public void ShowBannerAds()
    {
        Advertising.ShowBannerAd(BannerAdPosition.Bottom);
    }

    public bool ShowInterstitialAds()
    {
        if (Advertising.IsInterstitialAdReady())
        {
            Advertising.ShowInterstitialAd();
			return true;
        }

		return false;
    }

    public bool ShowRewardedAds()
    {
        if (Advertising.IsRewardedAdReady())
        {
            Advertising.ShowRewardedAd();
			return true;
        }

		return false;
    }

    public void ShowLeaderboard()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
    }

    public void ShowAchievement()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ShowAchievementsUI();
        }
    }

    public void SubmitScoreToLeaderboard(int value)
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ReportScore(value, EM_GameServicesConstants.Leaderboard_Ovni_Racer_Leaderboard);
        }
    }

    public void SubmitScoreToLeaderboardDebugButton()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ReportScore(100, EM_GameServicesConstants.Leaderboard_Ovni_Racer_Leaderboard);
        }
    }

    public void LoadLocalUserScoreDebugButton()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_Ovni_Racer_Leaderboard, OnLocalUserScoreLoaded);
        }
    }

    private void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
    {
        if (score != null)
        {
            userScoreText.text = "Your score is : " + score.value;
        }
        else
        {
            userScoreText.text = "You don't have any score reported to leaderboard " + leaderboardName;
        }
    }

    public void UnlockAchievement()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Ovni_Racer_Achievement1);
        }
    }
}
