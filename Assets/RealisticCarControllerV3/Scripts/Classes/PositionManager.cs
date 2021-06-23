using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public CarPositionManager[] Cars;
    List<CarPositionManager> carpos;

    public void SetCars() {

        Cars = GameObject.FindObjectsOfType<CarPositionManager>();

    }

    void Start()
    {
        carpos = new List<CarPositionManager>();

        foreach (CarPositionManager car in Cars)
        {
            carpos.Add(car);

        }

        foreach (CarPositionManager car in carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ToList())
        {
            Debug.Log(car);

        }




    }

    // Update is called once per frame
    void LateUpdate()
    {
          AssignPosition();
    }



    public void AssignPosition()
    {


        int pos = 0;
        foreach (CarPositionManager cpm in carpos.OrderByDescending(x => x.lapCounter).ThenByDescending(x => x.Counter).ToList())
        {

            pos++;
            if (cpm.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
            {

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

                break;
            }
        }

    }


}
