using UnityEngine;

[System.Serializable]
public class Passenger {
        
    public string name;

    public enum Type { 

        CASUAL,    
        OFFICEGUY,
        PREGNANTLADY,
        PILOT,
        THIEF,
        AIRPORT_PASSENGER,
        TEACHER,
        YOUNGSTER,
        SENIOR_CITIZEN        
    }
    public Type type = Type.CASUAL;
    public GameObject prefab;

    public string destination = "";
    public string description = "";

    public float speedLimit = 50;
    public int timeLimit = 60;

    [Header("Special Event")]
    public bool hasSpecialEvent = false;
    public bool startRaining = false;
    public string description_SE = "";

    public string destination_SE = "";
    public float speedLimit_SE = 50;
    public int timeLimit_SE = 60;
}

[CreateAssetMenu(fileName = "LevelData", menuName = ("Level Data"))]
public class LevelData : ScriptableObject
{
    public int allowedPanelties = 5;
    public bool isRaining = false;
    public bool isNight = false;
    public Passenger [] passenger;
}
