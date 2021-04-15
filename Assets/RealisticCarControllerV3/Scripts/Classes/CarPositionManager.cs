using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CarPositionManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int Counter = 0;
    public int lapCounter = 0;
    public float Distance = 0.0f;
    public String Position;
    public TextMesh TextMesh;
    bool[] myArray = new bool[10];
    bool lapCheck = true;
    CarPositionManager[] Cars;
    public List<CarPositionManager> carpos = new List<CarPositionManager>();
    List<PositionClass> PositionsList = new List<PositionClass>();
    
    void Start()
    {
        if (Toolbox.GameplayScript) {

            GameObject PositionSystem = Toolbox.GameplayScript.positionManager.gameObject;
            Cars = GameObject.FindObjectsOfType<CarPositionManager>();

            foreach (CarPositionManager car in Cars)
            {
                // if(this.gameObject.GetInstanceID()!=car.GetInstanceID())
                carpos.Add(car);

            }

            //for (int i = 0; i < PositionSystem.transform.childCount; ++i)
            //{
            //    PositionClass ps = new PositionClass(PositionSystem.transform.GetChild(i).gameObject, false);
            //    PositionsList.Add(ps);
            //}

            //Distance = Vector3.Distance(PositionsList.ElementAt(0).position.transform.position, this.gameObject.transform.position);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        CheckPosition(other.gameObject);

        if (other.CompareTag("FinishLine"))
        {
            //if (CheckAllCheckPointTeversed())
            //{
            //if (lapCheck)
            //{
            //    lapCounter++;
            //    DisableAllCheckPoints();
            //    Counter = 0;
            //    lapCheck = false;
            //}

            if (this.gameObject.CompareTag("Player"))
            {

                DisableAllCheckPoints();
                Counter = 0;
                lapCheck = false;

                Toolbox.GameplayScript.LevelCompleteHandling();
            }
            else { 
            

            }


            //}
        }


    }


    public bool CheckPosition(GameObject go)
    {
        int index = 0;
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
                Distance = Vector3.Distance(PositionsList.ElementAt(index).position.transform.position, this.gameObject.transform.position);
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

            if (ps.position.tag == "CheckPoint" && !ps.check)
            {
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

}
