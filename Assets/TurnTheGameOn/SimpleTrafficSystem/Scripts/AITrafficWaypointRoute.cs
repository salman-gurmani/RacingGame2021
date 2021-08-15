﻿namespace TurnTheGameOn.SimpleTrafficSystem
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

    [System.Serializable]
    [RequireComponent(typeof(AITrafficWaypointRouteInfo))]
    [HelpURL("https://simpletrafficsystem.turnthegameon.com/documentation/api/aitrafficwaypointroute")]
    public class AITrafficWaypointRoute : MonoBehaviour
    {
        public bool isRegistered { get; private set; }
        [Tooltip("Array of vehicles types that are allowed to spawn and merge onto this route.")]
        public AITrafficVehicleType[] vehicleTypes = new AITrafficVehicleType[1];
        [Tooltip("List of waypoints in the route that cars will use for path-finding.")]
        public List<CarAIWaypointInfo> waypointDataList;
        public bool stopForTrafficLight { get; private set; }
        [Tooltip("Startup traffic will use spawn points, instead of incrementally spawning every other waypoint.")]
        public bool useSpawnPoints;
        [Tooltip("AITrafficController trafficPrefabs array will populate spawnTrafficVehicles array.")]
        public bool spawnFromAITrafficController = false;
        [Tooltip("Amount of cars to spawn.")]
        public int spawnAmount = 1;
        [Tooltip("Array of traffic car prefabs instantiated to the route on startup.")]
        public GameObject[] spawnTrafficVehicles;
        [Tooltip("Reference to the route's AITrafficWaypointRouteInfo script.")]
        public AITrafficWaypointRouteInfo routeInfo;
        [Tooltip("Amount of cars allowed on the route by startup and pooling spawners.")]
        public int maxDensity = 10;
        public int currentDensity; // unreliable if checked outside of AITrafficController update loop
        public int previousDensity; // more reliable if checked outside of AITrafficController update loop

        [ContextMenu("SetMaxToChildSpawnPointCount")]
        public void SetMaxToChildSpawnPointCount()
        {
            int spawnPoints = GetComponentsInChildren<AITrafficSpawnPoint>().Length;
            maxDensity = spawnPoints;
        }

        private void Awake()
        {
            routeInfo = GetComponent<AITrafficWaypointRouteInfo>();
        }

        private void Start()
        {
            RegisterRoute();
            if (AITrafficController.Instance.usePooling == false)
            {
                SpawnTrafficVehicles();
            }
        }
        #region Traffic Control
        
        public void StopForTrafficlight(bool _stop)
        {
            stopForTrafficLight = routeInfo.stopForTrafficLight = _stop;
            routeInfo.enabled = _stop ? false : true;
        }

        public List<AITrafficSpawnPoint> spawnpoints = new List<AITrafficSpawnPoint>();

        public void SpawnTrafficVehicles()
        {
            if (spawnFromAITrafficController)
            {
                spawnTrafficVehicles = new GameObject[spawnAmount];
                for (int i = 0; i < spawnTrafficVehicles.Length; i++)
                {
                    int randomPoolIndex = UnityEngine.Random.Range(0, AITrafficController.Instance.trafficPrefabs.Length);
                    spawnTrafficVehicles[i] = AITrafficController.Instance.trafficPrefabs[randomPoolIndex].gameObject;
                }
            }
            if (useSpawnPoints)
            {
                spawnpoints = GetComponentsInChildren<AITrafficSpawnPoint>().ToList();
                for (int i = 0; i < spawnTrafficVehicles.Length; i++)
                {
                    if (spawnpoints.Count > 0)
                    {
                        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnpoints.Count - 2);
                        Vector3 spawnPosition = spawnpoints[randomSpawnPointIndex].transform.position;
                        Vector3 spawnOffset = new Vector3(0, -4, 0);
                        spawnPosition += spawnOffset;
                        GameObject spawnedTrafficVehicle = Instantiate(spawnTrafficVehicles[i], spawnPosition, spawnpoints[randomSpawnPointIndex].transform.rotation);
                        spawnedTrafficVehicle.GetComponent<AITrafficCar>().RegisterCar(this);
                        spawnedTrafficVehicle.transform.LookAt(spawnpoints[randomSpawnPointIndex].waypoint.onReachWaypointSettings.parentRoute.waypointDataList[spawnpoints[randomSpawnPointIndex].waypoint.onReachWaypointSettings.waypointIndexnumber]._transform);
                        spawnpoints.RemoveAt(randomSpawnPointIndex);
                    }
                }
            }
            else
            {
                for (int i = 0, j = 0; i < spawnTrafficVehicles.Length && j < waypointDataList.Count - 1; i++, j++)
                {
                    Vector3 spawnPosition = waypointDataList[j]._transform.position;
                    spawnPosition.y += 1;
                    GameObject spawnedTrafficVehicle = Instantiate(spawnTrafficVehicles[i], spawnPosition, waypointDataList[j]._transform.rotation);
                    spawnedTrafficVehicle.GetComponent<AITrafficCar>().RegisterCar(this);
                    spawnedTrafficVehicle.transform.LookAt(waypointDataList[j + 1]._transform);
                    j += 1; // increase j again tospawn vehicles with more space between
                }
            }
        }
        #endregion

        #region Unity Editor Helper Methods
        bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
        {
            return (
                Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f &&
                Vector3.Distance(A, B) >= Vector3.Distance(A, C) &&
                Vector3.Distance(A, B) >= Vector3.Distance(B, C)
                );
        }

#if UNITY_EDITOR
        public Transform ClickToSpawnNextWaypoint(Vector3 _position)
        {
            GameObject newWaypoint = Instantiate(STSRefs.AssetReferences._AITrafficWaypoint, _position, Quaternion.identity, gameObject.transform) as GameObject;
            CarAIWaypointInfo newPoint = new CarAIWaypointInfo();
            newPoint._name = newWaypoint.name = "AITrafficWaypoint " + (waypointDataList.Count + 1);
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<AITrafficWaypoint>();
            newPoint._waypoint.onReachWaypointSettings.waypointIndexnumber = waypointDataList.Count + 1;
            newPoint._waypoint.onReachWaypointSettings.parentRoute = this;
            newPoint._waypoint.onReachWaypointSettings.speedLimit = 25f;
            waypointDataList.Add(newPoint);
            return newPoint._transform;
        }

        public void ClickToInsertSpawnNextWaypoint(Vector3 _position)
        {
            bool isBetweenPoints = false;
            int insertIndex = 0;
            if (waypointDataList.Count >= 2)
            {
                for (int i = 0; i < waypointDataList.Count - 1; i++)
                {
                    Vector3 point_A = waypointDataList[i]._transform.position;
                    Vector3 point_B = waypointDataList[i + 1]._transform.position;
                    isBetweenPoints = IsCBetweenAB(point_A, point_B, _position);
                    insertIndex = i + 1;
                    if (isBetweenPoints) break;
                }
            }

            GameObject newWaypoint = Instantiate(STSRefs.AssetReferences._AITrafficWaypoint, _position, Quaternion.identity, gameObject.transform) as GameObject;
            CarAIWaypointInfo newPoint = new CarAIWaypointInfo();
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<AITrafficWaypoint>();
            newPoint._waypoint.onReachWaypointSettings.parentRoute = this;
            newPoint._waypoint.onReachWaypointSettings.speedLimit = 25f;
            if (isBetweenPoints)
            {
                newPoint._transform.SetSiblingIndex(insertIndex);
                newPoint._name = newWaypoint.name = "AITrafficWaypoint " + (insertIndex + 1);
                waypointDataList.Insert(insertIndex, newPoint);
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    int newIndexName = i + 1;
                    waypointDataList[i]._transform.gameObject.name = "AITrafficWaypoint " + newIndexName;
                    waypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber = i + 1;
                }
            }
            else
            {
                newPoint._name = newWaypoint.name = "AITrafficWaypoint " + (waypointDataList.Count + 1);
                newPoint._waypoint.onReachWaypointSettings.waypointIndexnumber = waypointDataList.Count + 1;
                waypointDataList.Add(newPoint);
            }
        }
#endif
        #endregion

        #region Gizmos
        private void OnDrawGizmos() { if (STSPrefs.routeGizmos) DrawGizmos(false); }
        private void OnDrawGizmosSelected() { if (STSPrefs.routeGizmos) DrawGizmos(true); }

        [HideInInspector] Transform arrowPointer;
        private Transform junctionPosition;
        private Matrix4x4 previousMatrix;
        private int lookAtIndex;

        private void DrawGizmos(bool selected)
        {
            if (!arrowPointer)
            {
                arrowPointer = new GameObject("ARROWPOINTER").transform;
                arrowPointer.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            // Draw line to new route points
            Gizmos.color = selected ? STSPrefs.selectedPathColor : STSPrefs.pathColor;
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                if (waypointDataList[i]._waypoint != null)
                {
                    Gizmos.color = selected ? STSPrefs.selectedJunctionColor : STSPrefs.junctionColor;
                    if (waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length > 0)
                    {
                        for (int j = 0; j < waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length; j++)
                        {
                            if (waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints[j] != null)
                            {
                                Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints[j].transform.position);
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            // Draw line to next waypoint and lane change points
            if (waypointDataList.Count > 1)
            {
                for (int i = 1; i < waypointDataList.Count; i++)
                {
                    Gizmos.color = selected ? STSPrefs.selectedPathColor : STSPrefs.pathColor;
                    Gizmos.DrawLine(waypointDataList[i - 1]._transform.position, waypointDataList[i]._transform.position); /// Line to next waypoint
                    if (waypointDataList[i - 1]._waypoint.onReachWaypointSettings.laneChangePoints != null)
                    {
                        for (int j = 0; j < waypointDataList[i - 1]._waypoint.onReachWaypointSettings.laneChangePoints.Count; j++) // lines to lane chane points
                        {
                            if (waypointDataList[i - 1]._waypoint.onReachWaypointSettings.laneChangePoints[j] != null)
                                Gizmos.DrawLine(waypointDataList[i - 1]._transform.position, waypointDataList[i - 1]._waypoint.onReachWaypointSettings.laneChangePoints[j].transform.position);
                        }
                    }
                }
            }

            // Draw Arrows to connecting waypoints
            if (waypointDataList.Count > 1)
            {
                Gizmos.color = selected ? STSPrefs.selectedPathColor : STSPrefs.pathColor;
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    previousMatrix = Gizmos.matrix;
                    if (waypointDataList[waypointDataList.Count - 2]._waypoint != null && waypointDataList[i]._waypoint != null)
                    {
                        arrowPointer.position = i == 0 ? waypointDataList[waypointDataList.Count - 2]._waypoint.transform.position : waypointDataList[i]._waypoint.transform.position;
                        lookAtIndex = i == 0 ? waypointDataList.Count - 1 : i - 1;
                        if (i == 0)
                        {
                            arrowPointer.LookAt(waypointDataList[waypointDataList.Count - 1]._waypoint.transform);
                            arrowPointer.position = waypointDataList[i]._waypoint.transform.position;
                            arrowPointer.Rotate(0, 180, 0);
                        }
                        else arrowPointer.LookAt(waypointDataList[lookAtIndex]._waypoint.transform);
                        Gizmos.matrix = Matrix4x4.TRS(waypointDataList[lookAtIndex]._waypoint.transform.position, arrowPointer.rotation, STSPrefs.arrowScale); // x, x, scale
                        Gizmos.DrawFrustum(Vector3.zero, 10f, 2f, 0f, 5f); // x, width, length, x, x
                    }
                    else
                    {
                        break;
                    }
                    previousMatrix = Gizmos.matrix;
                }
            }

            // Draw Arrows to junctions
            Gizmos.color = selected ? STSPrefs.selectedYieldTriggerColor : STSPrefs.yieldTriggerColor;
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                if (waypointDataList[i]._waypoint != null && waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints != null)
                {
                    for (int j = 0; j < waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints.Count; ++j)
                    {
                        if (waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints[j] != null)
                        {
                            Gizmos.color = selected ? STSPrefs.selectedPathColor : STSPrefs.pathColor;
                            previousMatrix = Gizmos.matrix;
                            junctionPosition = waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints[j].transform;
                            arrowPointer.position = waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints[j].transform.position; //waypointData [i]._transform.position;
                            arrowPointer.LookAt(waypointDataList[i]._transform);
                            Gizmos.matrix = Matrix4x4.TRS(junctionPosition.position, arrowPointer.rotation, STSPrefs.arrowScale); // x, x, scale
                            Gizmos.DrawFrustum(Vector3.zero, 10f, 2f, 0f, 5f); // x, width, length, x, x
                            Gizmos.matrix = previousMatrix;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if (routeInfo == null)
            {
                routeInfo = GetComponent<AITrafficWaypointRouteInfo>();
            }
        }
        #endregion

        #region Utility Methods
        public void RegisterRoute()
        {
            if (isRegistered == false)
            {
                AITrafficController.Instance.RegisterAITrafficWaypointRoute(this);
                isRegistered = true;
            }
        }

        public void RemoveRoute()
        {
            if (isRegistered)
            {
                AITrafficController.Instance.RemoveAITrafficWaypointRoute(this);
                isRegistered = false;
            }
        }

        [ContextMenu("ReversePoints")]
        public void ReversePoints()
        {
            List<CarAIWaypointInfo> reversedWaypointDataList = new List<CarAIWaypointInfo>();
            for (int i = waypointDataList.Count - 1; i >= 0; i--)
            {
                reversedWaypointDataList.Add(waypointDataList[i]);
            }
            for (int i = 0; i < reversedWaypointDataList.Count; i++)
            {
                reversedWaypointDataList[i]._transform.gameObject.name = "AITrafficWaypoint " + (i + 1).ToString();
                reversedWaypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber = i + 1;
                reversedWaypointDataList[i]._transform.SetSiblingIndex(i);
            }
            waypointDataList = reversedWaypointDataList;
        }

        [ContextMenu("AlignPoints")]
        public void AlignPoints()
        {
            for (int i = 0; i < waypointDataList.Count - 1; i++)
            {
                waypointDataList[i]._transform.LookAt(waypointDataList[i + 1]._transform);
            }
            if (waypointDataList.Count > 1)
            {
                waypointDataList[waypointDataList.Count - 1]._transform.rotation = waypointDataList[waypointDataList.Count - 2]._transform.rotation;
            }
        }

        [ContextMenu("RefreshPointIndexes")]
        public void RefreshPointIndexes()
        {
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                CarAIWaypointInfo waypointDataListItem = new CarAIWaypointInfo();
                waypointDataListItem._name = "AITrafficWaypoint " + (i + 1).ToString();
                waypointDataListItem._transform = waypointDataList[i]._transform;
                waypointDataListItem._waypoint = waypointDataList[i]._waypoint;
                waypointDataList[i] = waypointDataListItem;
                waypointDataList[i]._waypoint.gameObject.name = waypointDataList[i]._name;
                waypointDataList[i]._waypoint.onReachWaypointSettings.waypointIndexnumber = i + 1;
            }
            if (waypointDataList.Count >= 2)
            {
                waypointDataList[waypointDataList.Count - 1]._transform.LookAt(waypointDataList[waypointDataList.Count - 2]._transform);
            }
        }

        [ContextMenu("ClearAllLaneChangePoints")]
        public void ClearAllLaneChangePoints()
        {
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                waypointDataList[i]._waypoint.onReachWaypointSettings.laneChangePoints.Clear();
            }
        }

        [ContextMenu("ClearAllNewRoutePoints")]
        public void ClearAllNewRoutePoints()
        {
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints = new AITrafficWaypoint[0];
            }
        }

        public void RemoveAllSpawnPoints()
        {
            AITrafficSpawnPoint[] spawnPoints = GetComponentsInChildren<AITrafficSpawnPoint>();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Destroy(spawnPoints[i].gameObject);
            }
        }

#if UNITY_EDITOR
        public void SetupRandomSpawnPoints()
        {
            if (waypointDataList.Count > 4)
            {
                RemoveAllSpawnPoints();
                int randomIndex = UnityEngine.Random.Range(1, 3);
                for (int i = randomIndex; i < waypointDataList.Count && i < waypointDataList.Count - 3; i += UnityEngine.Random.Range(1, 3))
                {
                    GameObject loadedSpawnPoint = Instantiate(STSRefs.AssetReferences._AITrafficSpawnPoint, waypointDataList[i]._transform) as GameObject;
                    AITrafficSpawnPoint trafficSpawnPoint = loadedSpawnPoint.GetComponent<AITrafficSpawnPoint>();
                    trafficSpawnPoint.waypoint = trafficSpawnPoint.transform.parent.GetComponent<AITrafficWaypoint>();
                }
            }
        }
#endif
        #endregion
    }
}