﻿using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform[] aiSpawnPoints;

    public GameObject cinemachineObj;
    public GameObject raceCountdown;
    public GameObject heliObj;
    public float heliLife = 10f;
    public DistanceCalculation distScript;

    private bool isCountdownstart = false;


    private void Start()
    {
        cinemachineObj.SetActive(true);
        Toolbox.GameplayScript.camListner.GetComponent<Camera>().enabled = false;
    }

    public void Start_RaceCountdown()
    {
        isCountdownstart = true;

        if(raceCountdown)
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
        if (raceCountdown)
            raceCountdown.SetActive(false);

        Toolbox.HUDListner.controls.SetActive(true);
        Toolbox.GameplayScript.StartRaceHandling();
    }

    private void Disable_Heli()
    {
        heliObj.SetActive(false);
    }
}
