using UnityEngine;
using System.Collections;

public class LevelsManager : MonoBehaviour
{
    public bool testMode = false;

    private LevelHandler curLevelHandler;
    [SerializeField] private LevelData curLevelData;

    public LevelData CurLevelData { get => curLevelData; set => curLevelData = value; }
    public LevelHandler CurLevelHandler { get => curLevelHandler; set => curLevelHandler = value; }

    private void Start()
    {
        if (testMode)
        {
            curLevelHandler = this.GetComponentInChildren<LevelHandler>();

        }
        else {

            InstantiateLevel();
        }

        SpawnPlayer();
        LevelDataHandling();
    }

    private void InstantiateLevel()
    {
        string path = Constants.PrefabFolderPath + Constants.LevelsFolderPath + Toolbox.DB.prefs.LastSelectedLevel.ToString();
        //Toolbox.GameManager.Log("Lvl path = " + path);

        GameObject obj = (GameObject)Instantiate(Resources.Load(path), this.transform);
        
        curLevelHandler = obj.GetComponent<LevelHandler>();
    }

    private void SpawnPlayer()
    {
        string path = Constants.PrefabFolderPath + Constants.PlayerFolderPath + Toolbox.DB.prefs.LastSelectedPlayerObj.ToString();
        //Toolbox.GameManager.Log("Vehicle path = " + path);

        GameObject obj = (GameObject)Instantiate(Resources.Load(path), curLevelHandler.playerSpawnPoint.position, curLevelHandler.playerSpawnPoint.rotation);

        Toolbox.GameplayScript.EnableHud();

        Toolbox.GameplayScript.PlayerObject = obj;

        Toolbox.GameManager.Instantiate_Blackout();

        Toolbox.GameplayScript.cameraScript.playerCar = obj.transform;
        Toolbox.HUDListner.carController = obj.GetComponent<RCC_CarControllerV3>();
    }

    private void LevelDataHandling()
    {
        string path = Constants.PrefabFolderPath + Constants.LevelsScriptablesFolderPath + Toolbox.DB.prefs.LastSelectedLevel.ToString();
        //Toolbox.GameManager.Log("Level Data path = " + path);
        // All level specs related handling
        curLevelData = (LevelData)Resources.Load(path);

        if (curLevelData.isRaining)
            Toolbox.GameplayScript.RainStatus(true);

        Toolbox.HUDListner.SetTotalPanelties(curLevelData.allowedPanelties);

        Toolbox.HUDListner.SetLvlTxt("Level " + (Toolbox.DB.prefs.LastSelectedLevel + 1).ToString());
    }
}
