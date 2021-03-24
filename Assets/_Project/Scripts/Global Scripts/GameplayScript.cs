using System.Collections;
using UnityEngine;

/// <summary>
/// This script will hold everything that is needed to be global only in Game scene
/// </summary>


public class GameplayScript : MonoBehaviour {
    public bool testMode = true;
    public bool islevelsScene = true;
    public bool isRideStart = false;
    public bool isSE_implemented = false;

    public bool levelCompleted = false;
    public bool levelFailed = false;
    public bool stopmap;

    private int gameplayTime_Seconds = 0;

    private int score = 0;
    private int ridesCompleted = 0;

    private GameObject playerObject;
    public GameObject rainObj;
    public Material NightSkybox, DaySkybox;
    public GameObject NightLight, DayLight;
    
    [Header("MiniMap")]
    public InsaneSystems.RoadNavigator.Map map;
    public InsaneSystems.RoadNavigator.Navigator mapNavigator;

    [HideInInspector]
    private int levelCompleteTime = 0;

    [Header("Components")]
    public AudioListener camListner;
    public RCC_Camera cameraScript;
    public bool canShowReviewMenu = false;
    public LevelsManager levelsManager;
    public LookAtTarget carArrowScript;

    [Header("Colors")]
    public Color[] randomColors;

    //screenshot requirement
    int screenShotPicName = 0;

    public GameObject PlayerObject { get => playerObject; set => playerObject = value; }
    public int LevelCompleteTime { get => levelCompleteTime; set => levelCompleteTime = value; }

    void Awake() {

        //if (!FindObjectOfType<Toolbox>())
        //    Instantiate(Resources.Load("Toolbox"), Vector3.zero, Quaternion.identity);

        Toolbox.Set_GameplayScript(this.GetComponent<GameplayScript>());
    }

    void Start()
    {

        if (!Toolbox.DB.prefs.GameAudio)
            AudioListener.volume = 0.0f;

        Toolbox.GameManager.Analytics_LevelStart();

        AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);
        map.ShowAsNavigator();

        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);
        StartCoroutine(GameplayTime());
        levelCompleted = false;

        if (levelsManager.CurLevelData.isNight)
        {
            RenderSettings.skybox = NightSkybox;
            DayLight.SetActive(false);
            NightLight.SetActive(true);
        }
        else
        {
            RenderSettings.skybox = DaySkybox;
            NightLight.SetActive(false);
            DayLight.SetActive(true);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            string name = "Pic_" + screenShotPicName + ".png";
            Toolbox.GameManager.Log("Screenshot Taked!");
            ScreenCapture.CaptureScreenshot(name);
            screenShotPicName++;
        }
#endif
    }

    public void StartGame()
    {
        EnableHud();
        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);

        StartCoroutine(GameplayTime());
    }

    IEnumerator GameplayTime() {

        while (true) {

            yield return new WaitForSeconds(1);            
            gameplayTime_Seconds++;

            TImeRelatedFucntionsHandler();
        }

    }
    
    /// <summary>
    /// Functions that are called depending upon the gameplay time
    /// </summary>
    private void TImeRelatedFucntionsHandler()
    {

        if (gameplayTime_Seconds % 60 == 0)
        {
            canShowReviewMenu = true;
        }
    }

    public void IncrementGoldCoins(int cost)
    {
        Toolbox.DB.prefs.GoldCoins += cost;
    }

    public void DeductGoldCoins(int cost) {

        Toolbox.DB.prefs.GoldCoins -= cost;
    }

    public void IncrementScore(int _val) {

        score += _val;

        if (score >= levelsManager.CurLevelData.passenger.Length - 1)
        {
            LevelCompleteHandling();
            map.DisableMap();

        }
    }

    public void CompleteRideHandling() {

        ridesCompleted++;
        IncrementScore(1);
        isRideStart = false;
        Toolbox.HUDListner.StartTime = false;
        levelsManager.CurLevelHandler.EnablePassengerPoint(ridesCompleted);
    }

    public void EnableCurrentPassengerStats() {

        Toolbox.HUDListner.SetNStartTime(levelsManager.CurLevelData.passenger[ridesCompleted].timeLimit);
        Toolbox.HUDListner.SetSpeedLimit(levelsManager.CurLevelData.passenger[ridesCompleted].speedLimit);
        
    }

    public void InitEffectOnpoint(GameObject effect, Vector3 pos) {

        Instantiate(effect, pos, Quaternion.identity);
    }

    public void EnableHud() {
        Toolbox.HUDListner.gameObject.SetActive(true);
    }

    public void DisableHud() {

        Toolbox.HUDListner.ResetControls();
        Toolbox.HUDListner.gameObject.SetActive(false);
    }
    
    public Color Get_RandomColor() {

        return randomColors[Random.Range(0, randomColors.Length - 1)];
    }

    public void RideStartHandling() {

        if (levelsManager) {

            Passenger pas = levelsManager.CurLevelData.passenger[ridesCompleted];
            Toolbox.GameManager.InstantiateRide_Msg(pas.name, pas.destination, pas.description, pas.timeLimit, pas.speedLimit);

            Toolbox.HUDListner.SetNStartTime(pas.timeLimit);
            Toolbox.HUDListner.SetSpeedLimit(pas.speedLimit);
            isRideStart = true;
            Toolbox.GameplayScript.carArrowScript.Status(true);
        }
    }


    public void LevelCompleteHandling() {

        if (levelFailed || levelCompleted)
            return;

        Toolbox.GameplayScript.stopmap = true;

        levelCompleted = true;
        Toolbox.GameManager.Instantiate_LevelComplete(1);

        DisableHud();
    }

    public void LevelFailHandling() {

        if (levelFailed || levelCompleted)
            return;


        Toolbox.GameplayScript.stopmap = true;
        levelFailed = true;

        Toolbox.GameManager.Instantiate_LevelFail(1);
        DisableHud();
    }

    public void RainStatus(bool _val) {

        rainObj.SetActive(_val);
    }
}
