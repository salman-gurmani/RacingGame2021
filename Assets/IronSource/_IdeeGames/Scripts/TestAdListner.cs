using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdListner : MonoBehaviour
{
    public void ShowBanner() {

        AdsManager.instance.RequestBannerWithSpecs(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
    public void HideBanner()
    {
        AdsManager.instance.HideBannerAd();
    }

    public void ShowIAD()
    {
        AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
    }

    public void LoadIAD()
    {
        AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);
    }

    public void ShowRAD()
    {

        AdsManager.instance.ShowAd(AdsManager.AdType.REWARDED);
    }
}
