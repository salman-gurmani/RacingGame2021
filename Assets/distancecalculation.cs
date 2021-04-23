using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distancecalculation : MonoBehaviour
{
   // public Transform[] Points;
    public List<Transform> Point;
    float[] dist;
    public float accumulateDistance = 0;
    public int Index;
    // Start is called before the first frame update
    void Start()
    {
        Index = Toolbox.HUDListner.carController.gameObject.GetComponent<VehicleTriggerHandler>().current;
        //   dist =  Vector3.Distance(this.transform.position, NextGameObj.transform.position);
        Distance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Index < Toolbox.HUDListner.carController.gameObject.GetComponent<VehicleTriggerHandler>().current)
        {
            accumulateDistance = 0;
            Index += 1;
            Point.Remove(Toolbox.HUDListner.carController.gameObject.GetComponent<VehicleTriggerHandler>().hitObject.transform);
            Distance();
        }
    }
    public void Distance()
    {
        dist = new float [Point.Count + 1];
       
        for (int i = 0; i < Point.Count; ++i)
        {
            var t1 = Point[(i) % Point.Count];
            var t2 = Point[(i + 1) % Point.Count];
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.position;
                Vector3 p2 = t2.position;             
               // dist[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
            }
        }
    }
}
