using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.SocialPlatforms;
using TMPro;

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
    }

    public void ShowBannerAds()
    {
        Advertising.ShowBannerAd(BannerAdPosition.Bottom);
    }

    public void ShowInterstitialAds()
    {
        if (Advertising.IsInterstitialAdReady())
        {
            Advertising.ShowInterstitialAd();
        }
    }

    public void ShowRewardedAds()
    {
        if (Advertising.IsRewardedAdReady())
        {
            Advertising.ShowRewardedAd();
        }
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

    public void SubmitScoreToLeaderboard()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ReportScore(100, EM_GameServicesConstants.Leaderboard_OvniRacerLeaderboard);
        }
    }

    public void LoadLocalUserScore()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_OvniRacerLeaderboard, OnLocalUserScoreLoaded);
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
            GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_testAchievements);
        }
    }
}
