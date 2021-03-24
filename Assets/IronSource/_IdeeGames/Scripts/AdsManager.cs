using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public enum AdType { 
    
        BANNER,
        INTERSTITIAL,
        REWARDED,
        NATIVE
    };

    public enum RewardType
    {
        FREECOINS,
        DOUBLEREWARD,
        SKIPLEVEL,

    };

    private RewardType rewardType = RewardType.FREECOINS;
    private int coinsToReward = 0;

    private IronSourceBannerSize bannerAdSize;
    private IronSourceBannerPosition bannerAdPosition;

    private bool removeAdsPurchased = false;

    public RewardType RewardType1 { get => rewardType; set => rewardType = value; }
    public int CoinsToReward { get => coinsToReward; set => coinsToReward = value; }
    public bool RemoveAdsPurchased { get => removeAdsPurchased; set => removeAdsPurchased = value; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Log("Initializing");

#if UNITY_ANDROID
        string appKey = "e5850c39";
#elif UNITY_IPHONE
        string appKey = "e5850c39";
#else
        string appKey = "unexpected_platform";
#endif

        Log("IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Log("unity version" + IronSource.unityVersion());

        // SDK init
        Log("IronSource.Agent.init");
        IronSource.Agent.init(appKey);
    }

    void Log(string _str) {

        Debug.Log("Ads=" + _str);
     
    }

    void OnEnable()
    {
        Log("Registering Callbacks");

        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        ////Add Rewarded Video DemandOnly Events
        //IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent += RewardedVideoAdLoadedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
        //IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent += RewardedVideoAdLoadFailedDemandOnlyEvent;


        ////Add Offerwall Events
        //IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
        //IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
        //IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
        //IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
        //IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
        //IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;


        //Add Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        ////Add Interstitial DemandOnly Events
        //IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
        //IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
        //IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
        //IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
        //IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
        //IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;


        //Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
    }

    public void HideBannerAd() {


        Log("LoadBannerButtonClicked");
        IronSource.Agent.destroyBanner();
    }

    public void RequestBannerWithSpecs(IronSourceBannerSize _size, IronSourceBannerPosition _pos) {

        bannerAdSize = _size;
        bannerAdPosition = _pos;

        ShowAd(AdType.BANNER);
    }

    public void SetNShowRewardedAd(RewardType _type, int _coins)
    {
        rewardType = _type;
        coinsToReward = _coins;

        ShowAd(AdType.REWARDED);
    }



    public void RequestAd(AdType _type)
    {

        if (removeAdsPurchased)
        {
            Log("Ads Purchased. Not requesting Ad");
            return;
        }


        switch (_type)
        {
            case AdType.BANNER:

                break;

            case AdType.INTERSTITIAL:

                IronSource.Agent.loadInterstitial();

                break;

            case AdType.REWARDED:


                break;

            case AdType.NATIVE:


                break;
        }

    }


    public void ShowAd(AdType _type) 
    {

        if (removeAdsPurchased)
        {
            Log("Ads Purchased. Not showing Ad");
            return;
        }

        switch (_type)
        {

            case AdType.BANNER:

                IronSource.Agent.loadBanner(bannerAdSize, bannerAdPosition);

                break;

            case AdType.INTERSTITIAL:

                if (IronSource.Agent.isInterstitialReady())
                {
                    IronSource.Agent.showInterstitial();
                }
                else
                {
                    Log("IronSource.Agent.isInterstitialReady - False");
                }       

                break;

            case AdType.REWARDED:

                Log("ShowRewardedVideoButtonClicked");
                if (IronSource.Agent.isRewardedVideoAvailable())
                {
                    IronSource.Agent.showRewardedVideo();
                }
                else
                {
                    Log("IronSource.Agent.isRewardedVideoAvailable - False");
                }

                break;

            case AdType.NATIVE:

                break;
        }
    }


    #region OTHER METHODS

    void OnApplicationPause(bool isPaused)
    {
        Log("OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    /// <summary>
    /// Add the code here for Rewarded Ad
    /// </summary>
    void RewardPlayer()
    {
        Toolbox.DB.prefs.GoldCoins += coinsToReward;

        switch (rewardType)
        {

            case RewardType.FREECOINS:

                FindObjectOfType<MainMenuListner>().UpdateTxt();
                //Toolbox.GameManager.InstantiatePopup_Message(coinsToReward + " coins awarded.");

                break;

            case RewardType.DOUBLEREWARD:

                //Toolbox.GameManager.InstantiatePopup_Message(coinsToReward + "x2 coins awarded.");

                break;

            case RewardType.SKIPLEVEL:

                //FindObjectOfType<LevelFailListner>().UnlockAndPlayNextLevel();

                break;
        }
    }

    #endregion

    #region CallBacks

    #region BANNER

    void BannerAdLoadedEvent()
    {
        Log("BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Log("BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Log("BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Log("BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Log("BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Log("BannerAdLeftApplicationEvent");
    }

    #endregion

    #region INTERSTITIAL

    void InterstitialAdReadyEvent()
    {
        Log("InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Log("InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Log("InterstitialAdShowSucceededEvent");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Log("InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Log("InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Log("InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        Log("InterstitialAdClosedEvent");
    }

    /************* Interstitial DemandOnly Delegates *************/

    void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    {
        Log("InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Log("InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Log("InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    {
        Log("InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    {
        Log("InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    {
        Log("InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
    }

    #endregion

    #region REWARDED_VIDEO

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Log("RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        Log("RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Log("RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());

        RewardPlayer();
    }

    void RewardedVideoAdClosedEvent()
    {
        Log("RewardedVideoAdClosedEvent");
    }

    void RewardedVideoAdStartedEvent()
    {
        Log("RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        Log("RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Log("RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Log("RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    /************* RewardedVideo DemandOnly Delegates *************/

    void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
    {

        Log("RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {

        Log("RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
    {
        Log("RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
    {
        Log("RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
    {
        Log("RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Log("RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
    {
        Log("RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    #endregion

    #endregion

}
