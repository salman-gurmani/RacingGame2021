using UnityEngine;

public static class Constants{


#if UNITY_ANDROID
    public const string moreGamesLink = "https://play.google.com/store/apps/developer?id=RivalWheels";
    public const string appLink = "https://play.google.com/store/apps/details?id=com.rivalwheels.racing.car";

    public const string CP1 = "https://play.google.com/store/apps/details?id=com.supersmashact.citytaxidriver3d";
    public const string CP2 = "https://play.google.com/store/apps/details?id=com.rivalwheels.commando.secret.mission";
    public const string CP3 = "https://play.google.com/store/apps/details?id=com.rivalwheels.bussimulator2021";

#elif UNITY_IOS
    public const string moreGamesLink = "https://apps.apple.com/pk/developer/muhammad-salman-gurmani/id1549650722";
    public const string appLink = "https://apps.apple.com/pk/app/id1574141188";
        
    public const string CP1 = "https://apps.apple.com/pk/app/taxi-sim-2021/id1549650720";
    public const string CP2 = "https://apps.apple.com/pk/app/fps-battle-unleashed-2021/id1557610547";
    public const string CP3 = "https://apps.apple.com/pk/app/real-bus-driving-simulator-3d/id1560788544";
#endif

    public const string privacyPolicy = "https://docs.google.com/document/d/1rQcufgb_Y9noN3LNFnF5cvsMcldRyoSmP90XoT_H0_o/edit?usp=sharing";
    public const string fb = "";



    public const string serverPrefsLink = "";


    public const int iAd_SceneID = 0;

    public const int sceneIndex_Menu = 1;
    public const int sceneIndex_Game = 2;

#if UNITY_ANDROID

    public const string admobId_Banner = "ca-app-pub-8238060461072991/6149507541";
    public const string admobId_Interstitial = "ca-app-pub-8238060461072991/1879638785";
    public const string admobId_RewardedVid = "ca-app-pub-8238060461072991/8201219141";
    public const string admobId_Native = "ca-app-pub-8238060461072991/2210262530";

    public const string unityId_Appkey = "4184383";
    public const string unityId_IADkey = "Interstitial_Android";
    public const string unityId_RADkey = "Rewarded_Android";

#elif UNITY_IOS
    public const string admobId_Banner = "ca-app-pub-6351158520517644/8278384388";
    public const string admobId_Interstitial = "ca-app-pub-6351158520517644/5652221046";
    public const string admobId_RewardedVid = "ca-app-pub-6351158520517644/4339139374";
    public const string admobId_Native = "";

    public const string unityId_Appkey = "4184382";

#endif

    //TestID
    //public const string admobId_Banner = "ca-app-pub-3940256099942544/6300978111";
    //public const string admobId_Interstitial = "ca-app-pub-3940256099942544/1033173712";
    //public const string admobId_RewardedVid = "ca-app-pub-3940256099942544/5224354917";
    //public const string admobId_Native = "ca-app-pub-3940256099942544/2247696110";
 

#region InApp

    public const string coins_1 = "1k_cash";
    public const string coins_2 = "3k_cash";
    public const string coins_3 = "5k_cash";
    public const string coins_4 = "20k_cash";
    public const string coins_5 = "50k_cash";
    public const string unlockPlayerObj = "unlock_all_cars";
    public const string unlockLevels = "unlock_all_levels";
    public const string buyFuel = "refill_tank";

    //TestID
    //public const string admobId_Banner = "ca-app-pub-3940256099942544/6300978111";
    //public const string admobId_Interstitial = "ca-app-pub-3940256099942544/1033173712";
    //public const string admobId_RewardedVid = "ca-app-pub-3940256099942544/5224354917";
    //public const string admobId_Native = "ca-app-pub-3940256099942544/2247696110";

#endregion

#region ResourcesLinks

    public const string menuFolderPath = "Menues/";
    public const string PrefabFolderPath = "Prefabs/";
    public const string LevelsFolderPath = "Levels/";
    public const string LevelsScriptablesFolderPath = "LevelsScriptables/";
    public const string PlayerFolderPath = "PlayerObj/";
    public const string PlayerScriptablesFolderPath = "PlayerObjScriptables/";

#endregion
}
