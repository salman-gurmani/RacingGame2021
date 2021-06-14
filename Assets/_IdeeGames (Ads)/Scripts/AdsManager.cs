using Tapdaq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public Text testTxt;

    public enum AdType
    {

        BANNER,
        INTERSTITIAL,
        VIDEOINTERSTITIAL,
        REWARDED,
        NATIVE
    };

    public enum RewardType
    {
        FREECOINS,
        DOUBLEREWARD,
        SKIPLEVEL
    };
    private RewardType rewardType = RewardType.FREECOINS;
    private int coinsToReward = 0;

    private bool isInitialized = false;
    private bool iAdLoaded = false;
    private bool rAdLoaded = false;

    //private BannerView bannerView;
    //private InterstitialAd interstitial;
    //private RewardedAd rewardedAd;

    private TDMBannerSize bannerAdSize;
    private TDBannerPosition bannerAdPosition;

    private DateTime lastAdShownTime;
    [SerializeField] private bool initializeOnStart = true;
    public int CoinsToReward { get => coinsToReward; set => coinsToReward = value; }
    public RewardType RewardType1 { get => rewardType; set => rewardType = value; }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        TDCallbacks.TapdaqConfigLoaded += OnTapdaqConfigLoaded;
        TDCallbacks.TapdaqConfigFailedToLoad += OnTapdaqConfigFailToLoad;
        TDCallbacks.AdAvailable += OnAdAvailable;
        TDCallbacks.AdNotAvailable += OnAdNotAvailable;
        TDCallbacks.AdError += OnAdError;

        TDCallbacks.RewardVideoValidated += OnRewardVideoValidated;
    }

    private void OnDisable()
    {
        TDCallbacks.TapdaqConfigLoaded -= OnTapdaqConfigLoaded;
        TDCallbacks.TapdaqConfigFailedToLoad -= OnTapdaqConfigFailToLoad;
        TDCallbacks.AdAvailable -= OnAdAvailable;
        TDCallbacks.AdNotAvailable -= OnAdNotAvailable;
        TDCallbacks.AdError -= OnAdError;

        TDCallbacks.RewardVideoValidated -= OnRewardVideoValidated;
    }


    private void Start()
    {
        if (initializeOnStart)
            Initialize();
    }

    void Log(string _str)
    {

        //Toolbox.GameManager.Log("Ads=" + _str);
        //Debug.Log("Ads=" + _str);

        if (testTxt)
        {
            testTxt.text = _str;
        }
    }

    public void Initialize()
    {

        Log("Initializing");
        // Initialize the Google Mobile Ads SDK.

        AdManager.SetUserSubjectToGdprStatus(TDStatus.TRUE);
        AdManager.SetConsentStatus(TDStatus.TRUE);
        AdManager.SetAgeRestrictedUserStatus(TDStatus.TRUE);

        AdManager.Init();

        lastAdShownTime = DateTime.Now;
    }

    public void HideBannerAd()
    {
        Log("Hiding banner");
        AdManager.HideBanner();
    }

    public void RequestBannerWithSpecs(TDMBannerSize _size, TDBannerPosition _pos)
    {

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
        Log("Request Ad. Type = " + _type);

        //#if UNITY_EDITOR
        //        return;
        //#endif

        if (!isInitialized /*|| !Toolbox.GameManager.IsNetworkAvailable()*/)
        {
            Log("Not Initialized");
            return;
        }

        switch (_type)
        {
            case AdType.BANNER:

                if (!Toolbox.DB.prefs.NoAdsPurchased)
                {

                    HideBannerAd();

                    AdManager.RequestBanner(bannerAdSize);

                }

                break;

            case AdType.INTERSTITIAL:

                if (/*!iAdLoaded && */!Toolbox.DB.prefs.NoAdsPurchased)
                {
                    Log("Requesting Interstitial");

                    AdManager.LoadInterstitial("default");

                    AdManager.LoadInterstitial("Zone 1");

                    AdManager.LoadInterstitial("Zone 2");

                }

                break;

            case AdType.VIDEOINTERSTITIAL:

                if (/*!iAdLoaded && */!Toolbox.DB.prefs.NoAdsPurchased)
                {
                    Log("Requesting " + _type);

                    AdManager.LoadVideo("default");
                }

                break;


            case AdType.REWARDED:

                //if (rAdLoaded)
                //    return;

                AdManager.LoadRewardedVideo("default");

                break;

            case AdType.NATIVE:


                break;
        }

    }

    public void ShowAd(AdType _type)
    {

        //#if UNITY_EDITOR
        //        return;
        //#endif

        if (!isInitialized /*|| !Toolbox.GameManager.IsNetworkAvailable()*/) {
            Log("Not Initialized");
            return;
        }
        

        Log("Trying to Show = " + _type);

        switch (_type)
        {

            case AdType.BANNER:

                if (AdManager.IsBannerReady())
                {
                    AdManager.ShowBanner(bannerAdPosition);
                }
                else {

                    RequestAd(AdType.BANNER);
                }

                break;

            case AdType.INTERSTITIAL:

                //if (Toolbox.DB.prefs.NoAdsPurchased)
                //    return;
                #region Acctual Showing the IAD
                if (AdManager.IsInterstitialReady("default"))
                {
                    Log("IAD Show 0");
                    AdManager.ShowInterstitial("default");
                }
                else
                if (AdManager.IsInterstitialReady("Zone 1"))
                {
                    Log("IAD Show 1");
                    AdManager.ShowInterstitial("Zone 1");

                }
                else if (AdManager.IsInterstitialReady("Zone 2"))
                {
                    Log("IAD Show 2");
                    AdManager.ShowInterstitial("Zone 2");
                }
                else {

                    Log("IAD not Available!");

                    if (AdManager.IsVideoReady("default"))
                    {
                        Log("VAD Show");
                        AdManager.ShowVideo("default");
                    }
                }

                #endregion
                break;

            case AdType.VIDEOINTERSTITIAL:

                if (AdManager.IsVideoReady("default"))
                {
                    Log("VAD Show");
                    AdManager.ShowVideo("default");
                }

                break;

            case AdType.REWARDED:

                if (AdManager.IsRewardedVideoReady("default"))
                {
                    AdManager.ShowRewardVideo("default");
                }
                else { 
                    Log("RAD not Available!");

                }

                break;
        }
    }

#region HELPER METHODS
    public bool IsShowIAdTime()
    {
        return true;
    }

    //private AdRequest CreateAdRequest()
    //{
    //    return new AdRequest.Builder()
    //        //.AddTestDevice(AdRequest.TestDeviceSimulator)
    //        //.AddTestDevice("8B3629979090AE15BCE808275C2963A4")
    //        //.AddKeyword("unity-admob-sample")
    //        //.TagForChildDirectedTreatment(true)
    //        .Build();
    //}


    private void OnTapdaqConfigLoaded()
    {
        //Tapdaq Ready to use
        isInitialized = true;

        //AdManager.SetUserSubjectToGdprStatus(TDStatus.TRUE);
        //AdManager.SetConsentStatus(TDStatus.TRUE);
        //AdManager.SetAgeRestrictedUserStatus(TDStatus.TRUE);

        RequestAd(AdType.BANNER);
        RequestAd(AdType.INTERSTITIAL);
        RequestAd(AdType.VIDEOINTERSTITIAL);
        RequestAd(AdType.REWARDED);

        Log("IsInitialized!");
        //Toolbox.GameManager.InstantiatePopup_Message("Ads Initialized!");

        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, false, Toolbox.GameManager.SceneDelay);
    }

    private void OnTapdaqConfigFailToLoad(TDAdError error)
    {
        Log("Not Initialized!");
        //Toolbox.GameManager.InstantiatePopup_Message("Ads Not Initialized!");
        //Tapdaq failed to initialise
        isInitialized = false;

        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, false, Toolbox.GameManager.SceneDelay);

    }

    private void OnAdAvailable(TDAdEvent e)
    {
        Log("OnAD Available");

        if (e.IsBannerEvent())
        {
            Log("BAD Available");
        }
        
        if (e.IsVideoEvent())
        {
            Log("VAD Available ");
        }

        if (e.IsInterstitialEvent())
        {
            Log("IAD Available");
        }

        if (e.IsRewardedVideoEvent())
        {
            Log("RAD Available");
        }
    }

    private void OnAdNotAvailable(TDAdEvent e)
    {
        Log("OnAD not Available");

        if (e.IsBannerEvent())
        {
            Log("BAD NOFILL - " + e.error);
        }

        if (e.IsVideoEvent())
        {
            Log("VAD NOFILL - " + e.error);
        }

        if (e.IsInterstitialEvent())
        {
            Log("IAD NOFILL - " + e.error);
        }

        if (e.IsRewardedVideoEvent())
        {
            Log("RAD NOFILL - " + e.error);
        }
    }

    private void OnAdError(TDAdEvent e)
    {
        Log("OnAD Error");

        if (e.IsBannerEvent())
        {
            Log("BAD Error - " + e.error);
        }

        if (e.IsVideoEvent())
        {
            Log("VAD Error - " + e.error);
        }

        if (e.IsInterstitialEvent())
        {
            Log("IAD Error - " + e.error);
        }

        if (e.IsRewardedVideoEvent())
        {
            Log("RAD Error - " + e.error);
        }
    }

    #endregion

    #region CallBacks

    private void OnRewardVideoValidated(TDVideoReward videoReward)
    {
        //Debug.Log("I got " + videoReward.RewardAmount + " of " + videoReward.RewardName
        //        + "   tag=" + videoReward.Tag + " IsRewardValid " + videoReward.RewardValid + " CustomJson: " + videoReward.RewardJson);

        if (videoReward.RewardValid)
        {
            RewardPlayer();
        }
        else
        {
            //Reward is invalid, video may not have completed or an ad network may have refused to the provide reward
        }
    }

    void RewardPlayer()
    {
        Toolbox.DB.prefs.GoldCoins += coinsToReward;

        switch (rewardType)
        {

            case RewardType.FREECOINS:

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


}