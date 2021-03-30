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
    CarPositionManager thisObject;
    void Start()
    {

        GameObject PositionSystem = GameObject.FindGameObjectWithTag("PositionSystem");
        Cars = GameObject.FindObjectsOfType<CarPositionManager>();

        foreach (CarPositionManager car in Cars)
        {
            // if(this.gameObject.GetInstanceID()!=car.GetInstanceID())
            carpos.Add(car);

        }

        for (int i = 0; i < PositionSystem.transform.childCount; ++i)
        {

            PositionClass ps = new PositionClass(PositionSystem.transform.GetChild(i).gameObject, false);
            PositionsList.Add(ps);

        }
        Distance = Vector3.Distance(PositionsList.ElementAt(0).position.transform.position, this.gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {



    }


    private void OnTriggerEnter(Collider other)
    {
        CheckPosition(other.gameObject);
        //  AssignPosition();
        if (other.gameObject.tag == "FinishLine")
        {
            if (CheckAllCheckPointTeversed())
            {
                if (lapCheck)
                {

                    lapCounter++;
                    //if (this.gameObject.tag == "Player")
                    //{
                    //    GameManager.Instance.gameplayScript.AddMission();
                    //}
                    DisableAllCheckPoints();
                    Counter = 0;
                    lapCheck = false;
                }

            }

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
