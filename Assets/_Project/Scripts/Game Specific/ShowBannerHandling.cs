using UnityEngine;

public class ShowBannerHandling : MonoBehaviour
{
    private void OnEnable()
    {
        AdsManager.instance.RequestBannerWithSpecs( /*Tapdaq.TDMBannerSize.TDMBannerStandard, Tapdaq.TDBannerPosition.Top*/);    
    }

    //private void OnDisable()
    //{
    //    AdsManager.instance.HideBannerAd();
    //}

}
