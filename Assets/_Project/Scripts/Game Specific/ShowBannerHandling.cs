using UnityEngine;

public class ShowBannerHandling : MonoBehaviour
{
    private void OnEnable()
    {
        AdsManager.instance.RequestBannerWithSpecs(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
    }

    private void OnDisable()
    {
        AdsManager.instance.HideBannerAd();
    }

}
