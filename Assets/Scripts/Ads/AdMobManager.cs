using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager current;

    private void Awake()
    {
        current = this;
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
}
