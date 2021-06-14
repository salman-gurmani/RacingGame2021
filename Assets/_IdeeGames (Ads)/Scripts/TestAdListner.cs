using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdListner : MonoBehaviour
{

    public void LoadBAD()
    {
        AdsManager.instance.RequestAd(AdsManager.AdType.BANNER);
        AdsManager.instance.RequestBannerWithSpecs(Tapdaq.TDMBannerSize.TDMBannerStandard, Tapdaq.TDBannerPosition.Bottom);

    }

    public void ShowBanner() {

        AdsManager.instance.ShowAd(AdsManager.AdType.BANNER);
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

    public void LoadRAD()
    {
        AdsManager.instance.RequestAd(AdsManager.AdType.REWARDED);
    }


    public void ShowRAD()
    {
        AdsManager.instance.ShowAd(AdsManager.AdType.REWARDED);
    }

    public void ShowVAD()
    {
        AdsManager.instance.ShowAd(AdsManager.AdType.VIDEOINTERSTITIAL);
    }

    public void LoadVAD()
    {
        AdsManager.instance.RequestAd(AdsManager.AdType.VIDEOINTERSTITIAL);
    }
}
