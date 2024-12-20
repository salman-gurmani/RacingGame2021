﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public CarPositionManager[] Cars;
    List<CarPositionManager> carpos;

    public void SetCars() {

        Cars = GameObject.FindObjectsOfType<CarPositionManager>();
        carpos = new List<CarPositionManager>();

        foreach (CarPositionManager car in Cars)
        {
            carpos.Add(car);

        }
        int pos = 0;
        foreach (CarPositionManager cpm in carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ThenBy(x => x.Distance).ToList())
        {

            pos++;
            cpm.positionVal = pos;

            if (pos == 1)
            {
                cpm.txtMesh.text = "1st";
                cpm.Position = "1st";

            }
            else if (pos == 2)
            {
                cpm.txtMesh.text = "2nd";
                cpm.Position = "2nd";

            }
            else if (pos == 3)
            {
                cpm.txtMesh.text = "3rd";
                cpm.Position = "3rd";
            }
            else if (pos == 4)
            {
                cpm.txtMesh.text = "4th";
                cpm.Position = "4th";

            }
            else if (pos == 5)
            {
                cpm.txtMesh.text = "5th";
                cpm.Position = "5th";
            }
            else if (pos == 6)
            {
                cpm.txtMesh.text = "6th";
                cpm.Position = "6th";
            }
            else if (pos == 7)
            {
                cpm.txtMesh.text = "7th";
                cpm.Position = "7th";
            }


        }
        StartCoroutine(AssignPosition());
    }

    void Start()
    {

        //Cars = GameObject.FindObjectsOfType<CarPositionManager>();
        //carpos = new List<CarPositionManager>();

        //foreach (CarPositionManager car in Cars)
        //{
        //    carpos.Add(car);

        //}
        //int pos = 0;
        //foreach (CarPositionManager cpm in carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ThenBy(x => x.Distance).ToList())
        //{

        //    pos++;
        //    cpm.positionVal = pos;

        //    if (pos == 1)
        //    {
        //        cpm.TextMesh.text = "1st";
        //        cpm.Position = "1st";

        //    }
        //    else if (pos == 2)
        //    {
        //        cpm.TextMesh.text = "2nd";
        //        cpm.Position = "2nd";

        //    }
        //    else if (pos == 3)
        //    {
        //        cpm.TextMesh.text = "3rd";
        //        cpm.Position = "3rd";
        //    }
        //    else if (pos == 4)
        //    {
        //        cpm.TextMesh.text = "4th";
        //        cpm.Position = "4th";

        //    }
        //    else if (pos == 5)
        //    {
        //        cpm.TextMesh.text = "5th";
        //        cpm.Position = "5th";
        //    }
        //    else if (pos == 6)
        //    {
        //        cpm.TextMesh.text = "6th";
        //        cpm.Position = "6th";
        //    }
        //    else if (pos == 7)
        //    {
        //        cpm.TextMesh.text = "7th";
        //        cpm.Position = "7th";
        //    }


        //}
        //StartCoroutine(AssignPosition());


    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
       
      //  if (other.gameObject.name == "ColliderFront"|| other.gameObject.transform.root.tag == "Player")
         //   AssignPosition();
    }


    IEnumerator AssignPosition()
    {

        while (true)
        {
            //Debug.LogError("IN A");
            yield return new WaitForSeconds(0.25f);
            //Debug.LogError("IN B");

            int pos = 0;
            List<CarPositionManager> tmp = new List<CarPositionManager>();
            tmp = carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ThenBy(x=>x.Distance).ToList();
            //Debug.LogError("Tmp len = " + tmp.Count);

            foreach (CarPositionManager cpm in tmp)
            {
               
                pos++;
                cpm.positionVal = pos;

                //if (pos == 1)
                //{
                //    //cpm.TextMesh.text = "1st";
                //    cpm.SetPositionTxt("1st");
                //}
                //else if (pos == 2)
                //{
                //    cpm.txtMesh.text = "2nd";

                //}
                //else if (pos == 3)
                //{
                //    cpm.txtMesh.text = "3rd";
                //}
                //else if (pos == 4)
                //{
                //    cpm.txtMesh.text = "4th";

                //}
                //else if (pos == 5)
                //{
                //    cpm.txtMesh.text = "5th";
                //}
                //else if (pos == 6)
                //{
                //    cpm.txtMesh.text = "6th";
                //}
                //else if (pos == 7)
                //{
                //    cpm.txtMesh.text = "7th";
                //}

                if (pos == 1)
                {
                    cpm.SetPositionTxt("1st");
                }
                else if (pos == 2)
                {
                    cpm.SetPositionTxt("2nd");
                }
                else if (pos == 3)
                {
                    cpm.SetPositionTxt("3rd");
                }
                else if (pos == 4)
                {
                    cpm.SetPositionTxt("4th");
                }
                else
                {
                    cpm.SetPositionTxt("5th");
                }
            }
        }
    }
}
