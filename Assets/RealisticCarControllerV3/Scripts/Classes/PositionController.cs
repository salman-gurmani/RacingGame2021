using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public CarPositionManager[] Cars;
    List<CarPositionManager> carpos;
    void Start()
    {
        Cars = GameObject.FindObjectsOfType<CarPositionManager>();
        carpos = new List<CarPositionManager>();

        foreach (CarPositionManager car in Cars)
        {
            carpos.Add(car);

        }
        int pos = 0;
        foreach (CarPositionManager cpm in carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ThenBy(x =>x.Distance).ToList()) 
        {

            pos++;

            if (pos == 1)
            {
                cpm.TextMesh.text = "1st";
            }
            else if (pos == 2)
            {
                cpm.TextMesh.text = "2nd";

            }
            else if (pos == 3)
            {
                cpm.TextMesh.text = "3rd";
            }
            else if (pos == 4)
            {
                cpm.TextMesh.text = "4th";

            }
            else
            {
                cpm.TextMesh.text = "5th";
            }


        }
        StartCoroutine(AssignPosition());



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
            yield return new WaitForSeconds(0.25f);
            int pos = 0;
            List<CarPositionManager> tmp = new List<CarPositionManager>();
            tmp = carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ThenBy(x=>x.Distance).ToList();
            foreach (CarPositionManager cpm in tmp)
            {

                pos++;

                if (pos == 1)
                {
                    cpm.TextMesh.text = "1st";
                }
                else if (pos == 2)
                {
                    cpm.TextMesh.text = "2nd";

                }
                else if (pos == 3)
                {
                    cpm.TextMesh.text = "3rd";
                }
                else if (pos == 4)
                {
                    cpm.TextMesh.text = "4th";

                }
                else
                {
                    cpm.TextMesh.text = "5th";
                }


            }


        }
    }
}
