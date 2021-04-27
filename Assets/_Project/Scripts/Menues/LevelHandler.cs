using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public GameObject cinemachineObj;
    public GameObject raceCountdown;
    public GameObject heliObj;
    public float heliLife = 10f;
    public DistanceCalculation distScript;

    private bool isCountdownstart = false;

    public void Update()
    {
        if (!cinemachineObj.activeInHierarchy && !isCountdownstart) Start_RaceCountdown();
    }
    public void Start_RaceCountdown()
    {
        isCountdownstart = true;
        raceCountdown.SetActive(true);
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.raceSfx);
        Invoke("Disable_RaceCountdown", 4);
        if (heliObj) { 
            heliObj.SetActive(true);
            Invoke("Disable_Heli", 10);
        }
    }

    public void Disable_RaceCountdown()
    {
        raceCountdown.SetActive(false);
        Toolbox.GameplayScript.StartRaceHandling();
    }

    private void Disable_Heli()
    {
        heliObj.SetActive(false);
    }
}
