﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class CarPositionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int Counter = 0;
    public int lapCounter = 0;
    public float Distance = 0.0f;
    public int totalCheckPoints = 0;
    public int CheckPointTraversed = 0 ;
    public int positionVal = 0;
    public String Position;
    public TextMesh txtMesh;
    bool[] myArray = new bool[10];
    bool lapCheck = true;
    private int index = 0;
    CarPositionManager[] Cars;
    public List<CarPositionManager> carpos = new List<CarPositionManager>();
    List<PositionClass> PositionsList = new List<PositionClass>();

    public bool isMenuScene = false;
    public bool isPlayer = false;

    void Start()
    {
        if (Toolbox.MenuHandler)
        {
            isMenuScene = true;
        }
        else
        {
            isMenuScene = false;
        }

        if (isPlayer) {
            txtMesh.gameObject.SetActive(false);
        }

        GameObject PositionSystem = GameObject.FindGameObjectWithTag("PositionSystem");

        if (!PositionSystem)
            return;

        Cars = GameObject.FindObjectsOfType<CarPositionManager>();

        foreach (CarPositionManager car in Cars)
        {
            // if(this.gameObject.GetInstanceID()!=car.GetInstanceID())
            carpos.Add(car);

        }

        for (int i = 0; i < PositionSystem.transform.childCount; i++)
        {
        if (PositionSystem.transform.GetChild(i).gameObject.tag.Contains("CheckPoint"))
            totalCheckPoints++;

            PositionClass ps = new PositionClass(PositionSystem.transform.GetChild(i).gameObject, false);
            PositionsList.Add(ps);
        }
        if(PositionsList.Count>0)
        Distance = Vector3.Distance(PositionsList.ElementAt(0).position.transform.position, this.gameObject.transform.position);
    }

    public void SetPositionTxt(string txt) {

        if (isPlayer)
        {
            
            Toolbox.HUDListner.SetPlayerPos(txt);
        }
        else {
            
            txtMesh.text = txt;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {

            if (other.CompareTag("FinishLine"))
            {
                //Debug.LogError("FinishLine Passed!");
                this.GetComponent<CarAIControl>().enabled = false;

                return;
            }

            if (other.CompareTag("NOS"))
            {
                return;
            }


            if (!other.CompareTag("FinishPoint"))
                CheckPosition(other.gameObject);

            if (other.tag.Contains("FinishPoint"))
            {
                //Debug.Log("Coming in FinishLine");
                if (CheckAllCheckPointTeversed())
                {
                    if (lapCheck)
                    {
                        Debug.LogError("LapEndPoint! " + other.gameObject.name);

                        if (this.gameObject.GetComponent<VehicleTriggerHandler>()) { 
                        
                        } 


                        lapCounter++;
                        DisableAllCheckPoints();
                        Counter = 0;
                        lapCheck = false;

                        if (Toolbox.GameplayScript.levelsManager.CurLevelData.type == LevelData.LevelType.LAP) {

                            if (this.gameObject.GetComponent<VehicleTriggerHandler>())
                            {
                                if (lapCounter >= Toolbox.GameplayScript.levelsManager.CurLevelData.laps)
                                {
                                    Toolbox.GameplayScript.RaceEndHandling();
                                }
                                else
                                {
                                    Toolbox.HUDListner.SetLapTxt(lapCounter + 1);
                                }
                            }                            
                        }
                        
                    }

                    if (this.gameObject.CompareTag("Player"))
                    {
                        DisableAllCheckPoints();
                        Counter = 0;
                        lapCheck = false;

                        // Toolbox.GameplayScript.LevelCompleteHandling();
                    }
                }
            }

            if (other.CompareTag("CheckPoint"))
            {
                CheckPointTraversed++;
            }
        }
        catch (Exception ex)
        {

            Debug.Log("Some error in OnTriggerEnter");
        }

    }


    public bool CheckPosition(GameObject go)
    {
         index = 0;
     
        foreach (PositionClass ps in PositionsList)
        {


           
            if (go == ps.position)
            {

                if (!ps.check)
                {

                    Counter++;
                    lapCheck = true;
                    ps.check = true;

                }
                index++;
                return true;

            }
            index++;
        }
        return false;


    }
    public bool CheckAllCheckPointTeversed()
    {


        foreach (PositionClass ps in PositionsList)
        {
            Debug.Log("ps tag:" + ps.position.tag + "Check:" + ps.check);
            if (ps.position.tag == "CheckPoint" && !ps.check)
            {
                CheckPointTraversed = 0;
                return false;
                
            }
        }
        return true;


    }

    public void DisableAllCheckPoints()
    {


        foreach (PositionClass ps in PositionsList)
        {

            ps.check = false;
        }



    }
    private void Update()
    {
        if (isMenuScene)
            return;
       
        if (PositionsList != null && index < PositionsList.Count)
            Distance = Vector3.Distance(PositionsList.ElementAt(index).position.transform.position, this.gameObject.transform.position);
    }

}
