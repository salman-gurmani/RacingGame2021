using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

[System.Serializable]
public class GameData
{
    public string appBundle;
    public float showAdDelay = 60;
    public int[] showCpAtPositions;
    public int cpIndex = 0;
    public bool showBAds = true;
    public bool showIAds = true;

    public bool IsCrossPromotionAdIndex(int _val)
    {
        foreach (var item in showCpAtPositions)
        {
            if (item == _val)
            {

                return true;
            }
        }
        return false;
    }

}

[System.Serializable]
public class CrossPromotionData
{
    public string appBundle;
    public string appLink;
    public Texture tex;
    public string texturePath;
}

[System.Serializable]
public class ServerPrefs
{
    public GameData[] gameData;
    public CrossPromotionData[] cpData;
}

[System.Serializable]
public class GameMode
{
    [SerializeField] private bool [] levelUnlocked;
    [SerializeField] private int [] levelStars;

    public bool[] LevelUnlocked { get => levelUnlocked; set => levelUnlocked = value; }
    public int[] LevelStars { get => levelStars; set => levelStars = value; }

    public int GetLastUnlockedLevel() {

        for (int i = 0; i < LevelUnlocked.Length; i++)
        {
            if (!levelUnlocked[i])
                return i-1;
        }

        return levelUnlocked.Length - 1;
    }
}

[System.Serializable]
public class Prefs
{
    [SerializeField] private bool gameAudio = true;
    [SerializeField] private bool gameMusic = true;
    [SerializeField] private bool tiltControl = false;

    [SerializeField] private int goldCoins = 0;
    [SerializeField] private int highScore = 0;
    [SerializeField] private int difficulty = 0;

    [SerializeField] private int analytic_GameRunCount = 0;

    [SerializeField] private int lastSelectedPlayerObj = 0;
    [SerializeField] private int lastSelectedMode = 0;
    [SerializeField] private int lastSelectedLevel = 0;
    [SerializeField] private string lastSelectedVehicleName;
    [SerializeField] private int fuelTank = 0;

    [SerializeField] private float tiltSensitivity = 0;

    [SerializeField] private bool fbLoggedIn = false;

    [SerializeField] private bool firstRun = true;
    [SerializeField] private bool tutorialShowed = false;
    [SerializeField] private bool appRated = false;

    [SerializeField] private bool mode2Unlocked = false;

    [SerializeField] private bool isSteerControl = false;

    [Header(">> Levels & Stars array size must be equal! <<")]
    [SerializeField] private GameMode[] gameMode;

    [SerializeField] private bool[] playerObjectBought;

    //[SerializeField] private bool[] levelsUnlockedMode1;
    //[SerializeField] private int[] levelStarsMode1;

    //[SerializeField] private bool[] levelsUnlockedMode2;
    //[SerializeField] private int[] levelStarsMode2;

    [SerializeField] private bool noAdsPurchased = false;


    [SerializeField] private DateTime firstTimeOpenTime;
    [SerializeField] private DateTime lastNotificationFireTime;
    [SerializeField] private DateTime lastClaimedRewardTime;
    [SerializeField] private int rewardDay;
    public void UnlockAllPlayerObj()
    {
        for (int i = 0; i < playerObjectBought.Length; i++)
        {
            playerObjectBought[i] = true;
        }
    }

    public void UnlockAllLevels() {


        foreach (var item in GameMode)
        {
            for (int i = 0; i < item.LevelUnlocked.Length; i++)
            {
                item.LevelUnlocked[i] = true;
            }
        }

    }

    public bool GameAudio { get => gameAudio; set => gameAudio = value; }
    public bool GameMusic { get => gameMusic; set => gameMusic = value; }
    public int GoldCoins { get => goldCoins; set => goldCoins = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public int Difficulty { get => difficulty; set => difficulty = value; }
    public bool FbLoggedIn { get => fbLoggedIn; set => fbLoggedIn = value; }
    public bool FirstRun { get => firstRun; set => firstRun = value; }
    public bool TutorialShowed { get => tutorialShowed; set => tutorialShowed = value; }
    public bool AppRated { get => appRated; set => appRated = value; }
    public bool NoAdsPurchased { get => noAdsPurchased; set => noAdsPurchased = value; }
    public DateTime FirstTimeOpenTime { get => firstTimeOpenTime; set => firstTimeOpenTime = value; }
    public DateTime LastNotificationFireTime { get => lastNotificationFireTime; set => lastNotificationFireTime = value; }
    public int Analytic_GameRunCount { get => analytic_GameRunCount; set => analytic_GameRunCount = value; }
    public int LastSelectedPlayerObj { get => lastSelectedPlayerObj; set => lastSelectedPlayerObj = value; }
    public int LastSelectedMode { get => lastSelectedMode; set => lastSelectedMode = value; }
    public bool Mode2Unlocked { get => mode2Unlocked; set => mode2Unlocked = value; }
    public int LastSelectedLevel { get => lastSelectedLevel; set => lastSelectedLevel = value; }
    public GameMode[] GameMode { get => gameMode; set => gameMode = value; }
    public bool[] PlayerObjectBought { get => playerObjectBought; set => playerObjectBought = value; }
    public bool IsSteerControl { get => isSteerControl; set => isSteerControl = value; }

    public DateTime LastClaimedRewardTime { get => lastClaimedRewardTime; set => lastClaimedRewardTime = value; }
    public int RewardDay { get => rewardDay; set => rewardDay = value; }
    public int FuelTank { get => fuelTank; set => fuelTank = value; }
    public string LastSelectedVehicleName { get => lastSelectedVehicleName; set => lastSelectedVehicleName = value; }
    public bool TiltControl { get => tiltControl; set => tiltControl = value; }
    public float TiltSensitivity { get => tiltSensitivity; set => tiltSensitivity = value; }
}
public class DB : MonoBehaviour {
       
    public Prefs prefs;
    public ServerPrefs serverPrefs;
    private GameData mygameData;
    public bool getServerData = false;

    private void Awake()
    {
        Load_Binary_Prefs();

        //if (getServerData && Application.internetReachability != NetworkReachability.NotReachable)
        //{
        //    StartCoroutine(GetOnlineData());
        //}
    }

    public bool isServerPrefAvailable()
    {

        if (serverPrefs.gameData.Length > 0 && serverPrefs.cpData.Length > 0)
            return true;
        else
            return false;
    }

    public GameData GetMyAppGameData()
    {
        if (mygameData == null)
        {
            string appId = Application.identifier;

            for (int i = 0; i < serverPrefs.gameData.Length; i++)
            {
                if (appId == serverPrefs.gameData[i].appBundle)
                {
                    mygameData = serverPrefs.gameData[i];
                }
            }
        }
        return mygameData;
    }

    //IEnumerator GetCrossPromotionTexture()
    //{
    //    //UnityWebRequest www = UnityWebRequestTexture.GetTexture(onlineConstants.crossPromotion_ImageURL);
    //    //yield return www.SendWebRequest();

    //    //if (www.isNetworkError || www.isHttpError)
    //    //{
    //    //    Debug.Log(www.error);
    //    //}
    //    //else
    //    //{
    //    //    crossPromotionTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //    //}
    //}


    #region OnlineData_Corouteens

    IEnumerator GetOnlineData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Constants.serverPrefsLink))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log("Error: " + www.error);
            }
            else
            {
                try
                {
                    serverPrefs = JsonUtility.FromJson<ServerPrefs>(www.downloadHandler.text);

                    if (serverPrefs.cpData.Length > 0)
                        StartCoroutine(GetCrossPromotionTexture(0));

                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    IEnumerator GetCrossPromotionTexture(int curIndex)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(serverPrefs.cpData[curIndex].texturePath);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            serverPrefs.cpData[curIndex].tex = ((DownloadHandlerTexture)www.downloadHandler).texture;

            if (curIndex + 1 < serverPrefs.cpData.Length)
                StartCoroutine(GetCrossPromotionTexture(curIndex + 1));
            //else
            //    Toolbox.AdsManager.Initialize();

        }
    }

    #endregion

    #region Calculation Functions


    #endregion

    #region Save/Load FUNCTIONS

    //public void Save_Prefs()
    //{
    //    string path = Get_FilePath();

    //    string jsonString = JsonUtility.ToJson(prefs);
    //    File.WriteAllText(path, jsonString);
    //}
    //private void Load_Prefs()
    //{
    //    string path = Get_FilePath();

    //    if (File.Exists(path))
    //    {
    //        string jsonString = File.ReadAllText(path);
    //        prefs = JsonUtility.FromJson<Prefs>(jsonString);
    //    }
    //    else
    //    {

    //        string jsonString = JsonUtility.ToJson(prefs);
    //        File.WriteAllText(path, jsonString);
    //    }
    //}

    public void Save_Binary_Prefs()
    {
        string path = Get_FilePath();

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create);

        formatter.Serialize(file, prefs);

        file.Close();
    }

    public void Load_Binary_Prefs()
    {
        string path = Get_FilePath();

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);

            Prefs tempPrefs = (Prefs)formatter.Deserialize(file);

            if (isArrayStructureSame(tempPrefs))
            {
                prefs = tempPrefs;
            }
            else {

                HandleChanges(tempPrefs);
            }

            file.Close();
        }
        else
        {
            Save_Binary_Prefs();
        }
    }


    /// <summary>
    /// Change in last two functions on adding any new Array item
    /// </summary>
    private void HandleChanges(Prefs _loadedPrefs)
    {
        //Copying Loaded Prefs Data into Current Prefs Array
        //if (prefs.GameMode.Length == _loadedPrefs.GameMode.Length) {

            GameMode[] tempGameMode = prefs.GameMode;

            for (int i = 0; i < _loadedPrefs.GameMode.Length; i++)
            {

                for (int j = 0; j < _loadedPrefs.GameMode[i].LevelUnlocked.Length; j++)
                {
                    tempGameMode[i].LevelUnlocked[j] = _loadedPrefs.GameMode[i].LevelUnlocked[j];
                }

                for (int k = 0; k < _loadedPrefs.GameMode[i].LevelStars.Length; k++)
                {
                    tempGameMode[i].LevelStars[k] = _loadedPrefs.GameMode[i].LevelStars[k];
                }
            }

            //Add other array handlings here

            prefs = _loadedPrefs;

            prefs.GameMode = tempGameMode;

            //for (int i = 0; i < prefs.GameMode.Length; i++)
            //{
            //    prefs.GameMode[i].LevelUnlocked = tempGameMode[i].LevelUnlocked;
            //    prefs.GameMode[i].LevelStars = tempGameMode[i].LevelStars;
            //}

        //}

        //bool[] t_LevelsUnlockedMode2 = new bool[prefs.LevelsUnlockedMode2.Length];
        //for (int i = 0; i < _loadedPrefs.LevelsUnlockedMode2.Length; i++)
        //{
        //    t_LevelsUnlockedMode2[i] = _loadedPrefs.LevelsUnlockedMode2[i];
        //}

        //prefs.LevelsUnlockedMode1 = t_LevelsUnlockedMode1;
    }

    private bool isArrayStructureSame(Prefs _loadedPref) {

        try
        {
            if (prefs.GameMode.Length > _loadedPref.GameMode.Length) {

                return false;
            }
            
            for (int i = 0; i < prefs.GameMode.Length; i++)
            {
                if (prefs.GameMode[i].LevelUnlocked.Length > _loadedPref.GameMode[i].LevelUnlocked.Length)
                    return false;
            }

            return true;
            
        }
        catch (Exception ex) {

            Debug.LogError("Exception In Array Structure Check. Error = " + ex);

            return false;
        }

    }

    string Get_FilePath()
    {
        return Application.persistentDataPath + "/file";
    }

    #endregion

    #region DebugArea Actions

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Screenshot Captured!");
            ScreenCapture.CaptureScreenshot("Screenshot_" + DateTime.Now + ".png");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {

        }

        if (Input.GetKeyDown(KeyCode.F3))
        {

        }
    }


    #endregion
}
