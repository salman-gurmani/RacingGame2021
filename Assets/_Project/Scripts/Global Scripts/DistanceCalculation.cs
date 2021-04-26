using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculation : MonoBehaviour
{
    public List<Transform> points; // Transform[] points;
    public float[] distance;
    public float accumulateDistance = 0;
    private Vector3[] mainPoints;
    public float mainDistance, LessDistance;
    // Start is called before the first frame update
    void Start()
    {
        calculatingDistance();
        mainDistance = accumulateDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void calculatingDistance()
    {
        accumulateDistance = 0;
        mainPoints = new Vector3[points.Count + 1];
        distance = new float[points.Count + 1];
        for (int i = 0; i < points.Count; ++i)
        {
            
            var t1 = points[(i) % points.Count];
            var t2 = points[(i + 1) % points.Count];
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.position;
                Vector3 p2 = t2.position;
                mainPoints[i] = points[i % points.Count].position;
                distance[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
                
            }
        }
    }
}
