﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
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

    private bool isLevelStatsAssigned = false;

    public int gameplayTime_Seconds = 0;

    private int score = 0;
    private int ridesCompleted = 0;

    private GameObject playerObject;
    public GameObject rainObj;
    public Canvas mapCanvas;
    public Material NightSkybox, DaySkybox;
    public GameObject NightLight, DayLight;
    
    [HideInInspector]
    private int levelCompleteTime = 0;
    private int remainingTime = 0;
    public int playerPositionVal;

    [Header("Components")]
    public AudioListener camListner;
    public RCC_Camera cameraScript;
    public bl_MiniMap map;
    public bool canShowReviewMenu = false;
    
    public GameObject circuit;

    public LevelsManager levelsManager;
    public PositionController positionManager;
    public List<GameObject> aiCars = new List<GameObject>();

    [Header("Colors")]
    public Color[] randomColors;

    //screenshot requirement
    int screenShotPicName = 0;

    public GameObject PlayerObject { get => playerObject; set => playerObject = value; }
    public int LevelCompleteTime { get => levelCompleteTime; set => levelCompleteTime = value; }
    public int RemainingTime { get => remainingTime; set => remainingTime = value; }

    void Awake() {

        //if (!FindObjectOfType<Toolbox>())
        //    Instantiate(Resources.Load("Toolbox"), Vector3.zero, Quaternion.identity);

        Toolbox.Set_GameplayScript(this.GetComponent<GameplayScript>());
    }

    void Start()
    {

        if (!Toolbox.DB.prefs.GameAudio)
            AudioListener.volume = 0.0f;
        if (Toolbox.DB.prefs.FuelTank > 0) Toolbox.DB.prefs.FuelTank -= 1;

        Toolbox.GameManager.Analytics_LevelStart();

        AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);
        //map.ShowAsNavigator();

        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);
        levelCompleted = false;

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

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            StartRaceHandling();
        }
#endif
        
    }

    public void StartGame()
    {
        Toolbox.HUDListner.EnableHud();
        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);

        StartCoroutine(GameplayTime());
    }

    IEnumerator GameplayTime() {

        while (true) {

            yield return new WaitForSeconds(1);

            if (levelFailed || levelCompleted)
            {

            }
            else {
                gameplayTime_Seconds++;
                TImeRelatedFucntionsHandler();
            }

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

    }

    public void EnableCurrentPassengerStats() {
                
    }

    public void EnableLevelStats()
    {
    }

    public void InitEffectOnpoint(GameObject effect, Vector3 pos) {

        Instantiate(effect, pos, Quaternion.identity);
    }

    
    public Color Get_RandomColor() {

        return randomColors[Random.Range(0, randomColors.Length - 1)];
    }

   

    public void LevelCompleteHandling() {

        if (levelFailed || levelCompleted)
            return;


        Toolbox.GameplayScript.stopmap = true;

        LevelCompleteTime = (int) Toolbox.HUDListner.TempTime;
        levelCompleted = true;
        Toolbox.GameManager.Instantiate_LevelComplete(1);
        Toolbox.HUDListner.DisableHUD();
    }

    public void LevelFailHandling() {

        if (levelFailed || levelCompleted)
            return;

        remainingTime = (int)Toolbox.HUDListner.TempTime;

        Toolbox.GameplayScript.stopmap = true;
        levelFailed = true;

        Toolbox.GameManager.Instantiate_LevelFail(1);
        Toolbox.HUDListner.DisableHUD();
    }

    public void RainStatus(bool _val) {

        rainObj.SetActive(_val);
    }

    public void AddAiCar(GameObject _val) {

        aiCars.Add(_val);
    }

    public void StartRaceHandling() {

        //Debug.LogError("Race Start!");

        foreach (var item in aiCars)
        {
            item.GetComponent<CarAIControl>().enabled = true;
            item.GetComponent<Rigidbody>().isKinematic = false;
        }

       // Toolbox.HUDListner.OnPress_Forward();
        if (!isLevelStatsAssigned)
        {
            EnableLevelStats();
            isLevelStatsAssigned = true;
        }

        Toolbox.HUDListner.EnableHud();
        Toolbox.GameplayScript.StartGame();

        if (mapCanvas) {

            map.Target = PlayerObject.transform;
            mapCanvas.enabled = true;
        }
        

        switch (levelsManager.CurLevelData.type) {

            case LevelData.LevelType.SPRINT:

                break;

            case LevelData.LevelType.LAP:

                Toolbox.HUDListner.SetLapTxt(1);

                break;

            case LevelData.LevelType.TIMESPRINT:

                PlayerObject.GetComponent<CarPositionManager>().txtMesh.gameObject.SetActive(false);
                Toolbox.HUDListner.SetNStartTime(levelsManager.CurLevelData.levelTime);
                Toolbox.HUDListner.playerCarPosition.gameObject.SetActive(false);
                Toolbox.HUDListner.pos.gameObject.SetActive(false);
                break;
        }
    }

    public void RaceEndHandling()
    {
        PlayerObject.GetComponent<CarPositionManager>().txtMesh.gameObject.SetActive(false);
        playerPositionVal = PlayerObject.GetComponent<CarPositionManager>().positionVal;



        switch (levelsManager.CurLevelData.type)
        {

            case LevelData.LevelType.SPRINT:

                if (playerPositionVal > 3)
                {
                    LevelFailHandling();
                }
                else
                {

                    LevelCompleteHandling();
                }

                break;

            case LevelData.LevelType.LAP:

                if (playerPositionVal > 3)
                {
                    LevelFailHandling();
                }
                else
                {
                    LevelCompleteHandling();
                }

                break;

            case LevelData.LevelType.TIMESPRINT:

                if (Toolbox.HUDListner.outofTime)
                {
                    LevelFailHandling();
                }
                else
                {

                    LevelCompleteHandling();
                }
                break;
        }
    }
}
