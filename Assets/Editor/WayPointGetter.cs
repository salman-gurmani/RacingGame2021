using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class WayPointGetter
{

    private static Transform [] Waypoints;
    private static Vector3[] points;
    private static  float[] distances;
    private static float Length;
    private static int p0n;
    private static int p1n;
    private static int p2n;
    private static int p3n;

    private static float i;
    private static Vector3 P0;
    private static Vector3 P1;
    private static Vector3 P2;
    private static Vector3 P3;
    private static int numPoints;
    [MenuItem("Tools/Racing Game Utilities/Car Position System")]
    private static void NewMenuOption()
    {
        int count = 0;
        float  distance;
        float lerpValue=0.0f;
        
      
         float editorVisualisationSubsteps = 500;

        List<Transform> listOfGameObjects = new List<Transform>();
        GameObject Waypoint=GameObject.FindGameObjectWithTag("WayPoint");
       
        for (int i = 0; i < Waypoint.transform.childCount; ++i)
        {
            listOfGameObjects.Add(Waypoint.transform.GetChild(i));
        }

        Waypoints=listOfGameObjects.ToArray();








        if (Waypoints.Length > 1)
        {

            if (GameObject.FindGameObjectWithTag("PositionSystem"))
            {

                Debug.LogError("Position System Already Exist");

            }
            else
            {




                GameObject cubeParent = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeParent.tag = "PositionSystem";
                cubeParent.name = "PositionSystem";
                numPoints = Waypoints.Length;
                cubeParent.AddComponent<PositionController>();
                CachePositionsAndDistances();
                Length = distances[distances.Length - 1];


                cubeParent.transform.position = new Vector3(0, 0, 0);
                Vector3 prev = Waypoints[0].position;
                int lookatIndex = 0;
                int nameIndex = 0;
                for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
                {
                    Vector3 next = GetRoutePosition(dist + 1);

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "" + nameIndex++;

                    cube.GetComponent<MeshRenderer>().enabled = false;
                    cube.transform.localScale = new Vector3(70, 40, 1);


                    cube.transform.LookAt(Waypoints[lookatIndex].transform);
                    cube.transform.GetComponent<BoxCollider>().isTrigger = true;
                    cube.transform.position = next;

                    cube.transform.parent = cubeParent.transform;


                }



            }





            SetCubeRotation();



        }
    }

    private static void SetCubeRotation()
    {
        List<Transform> listOfCubes = new List<Transform>();
        Transform[] CubeArray;
        GameObject Cube = GameObject.FindGameObjectWithTag("PositionSystem");
        for (int i = 0; i < Cube.transform.childCount; ++i)
        {
            
            listOfCubes.Add(Cube.transform.GetChild(i));



        }

        CubeArray=listOfCubes.ToArray();


        for(int i=0; i < CubeArray.Length; i++)
        {

            if (i == CubeArray.Length-1)
            {

                CubeArray[i].LookAt(CubeArray[0]);
            }
            else
            {
                CubeArray[i].LookAt(CubeArray[i + 1]);
            }
            
        }
        
     

    }
    private static void  CachePositionsAndDistances()
    {
        // transfer the position of each point and distances between points to arrays for
        // speed of lookup at runtime
        points = new Vector3[Waypoints.Length + 1];
        distances = new float[Waypoints.Length + 1];

        float accumulateDistance = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            var t1 = Waypoints[(i) % Waypoints.Length];
            var t2 = Waypoints[(i + 1) % Waypoints.Length];
            if (t1 != null && t2 != null)
            {
                Vector3 p1 = t1.position;
                Vector3 p2 = t2.position;
                points[i] = Waypoints[i % Waypoints.Length].position;
                distances[i] = accumulateDistance;
                accumulateDistance += (p1 - p2).magnitude;
            }
        }
    }
    private static Vector3 GetRoutePosition(float dist)
    {
        int point = 0;

        if (Length == 0)
        {
            Length = distances[distances.Length - 1];
        }

        dist = Mathf.Repeat(dist, Length);

        while (distances[point] < dist)
        {
            ++point;
        }


        // get nearest two points, ensuring points wrap-around start & end of circuit
        p1n = ((point - 1) + numPoints) % numPoints;
        p2n = point;

        // found point numbers, now find interpolation value between the two middle points

        i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

      
        
            // smooth catmull-rom calculation between the two relevant points


            // get indices for the surrounding 2 points, because
            // four points are required by the catmull-rom function
            p0n = ((point - 2) + numPoints) % numPoints;
            p3n = (point + 1) % numPoints;

            // 2nd point may have been the 'last' point - a dupe of the first,
            // (to give a value of max track distance instead of zero)
            // but now it must be wrapped back to zero if that was the case.
            p2n = p2n % numPoints;

            P0 = points[p0n];
            P1 = points[p1n];
            P2 = points[p2n];
            P3 = points[p3n];

            return CatmullRom(P0, P1, P2, P3, i);
        
        
    }

    private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
    {
        // comments are no use here... it's the catmull-rom equation.
        // Un-magic this, lord vector!
        return 0.5f *
               ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
    }

    [MenuItem("Tools/Racing Game Utilities/Populate Checkpoints")]
    static void InstantiatePrefab()
    {
        GameObject CheckPoints = new GameObject("CheckPoints");
        CheckPoints.tag = "Checkpoints";
        GameObject Cube = GameObject.FindGameObjectWithTag("PositionSystem");
        UnityEngine.Object PrefabBa = Selection.activeObject;
        int CheckpointIndex = 0;
      //  BoxCollider col=Cube.transform.GetChild(2).GetComponent<BoxCollider>();
        //var trans = col.transform;
        //var min = col.center - col.size * 0.5f;
        //var max = col.center + col.size * 0.5f;
        //var P000 = trans.TransformPoint(new Vector3(min.x, min.y, min.z));
        //var P001 = trans.TransformPoint(new Vector3(min.x, min.y, max.z));
        //var P010 = trans.TransformPoint(new Vector3(min.x, max.y, min.z));
        //var P011 = trans.TransformPoint(new Vector3(min.x, max.y, max.z));
        //var P100 = trans.TransformPoint(new Vector3(max.x, min.y, min.z));
        //var P101 = trans.TransformPoint(new Vector3(max.x, min.y, max.z));
        //var P110 = trans.TransformPoint(new Vector3(max.x, max.y, min.z));
        //var P111 = trans.TransformPoint(new Vector3(max.x, max.y, max.z));






        for (int i = 0; i < Cube.transform.childCount;i=i+7)
        {

         
            GameObject TriggerPoint=Cube.transform.GetChild(i).gameObject;
            TriggerPoint.tag = "CheckPoint";
            GameObject Checkpoint=UnityEngine.Object.Instantiate(PrefabBa, CheckPoints.transform) as GameObject;
            Checkpoint.name = "CheckPoint "+ ++CheckpointIndex;
            Checkpoint.transform.position = TriggerPoint.transform.position;
            Checkpoint.transform.rotation = TriggerPoint.transform.rotation;
            
            
        }


        //Selection.activeObject = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);



    }
    [MenuItem("Tools/Racing Game Utilities/Populate FinishLine")]
    static void InstantiateFinishLine()
    {
        Selection.activeObject = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);
    }
    [MenuItem("Tools/Racing Game Utilities/Populate Checkpoints", true)]
    static bool ValidateInstantiatePrefab()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

    [MenuItem("Tools/Racing Game Utilities/Populate FinishLine", true)]
    static bool ValidateInstantiatePrefabFinishLine()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go == null)
            return false;

        return PrefabUtility.IsPartOfPrefabAsset(go);
    }

}