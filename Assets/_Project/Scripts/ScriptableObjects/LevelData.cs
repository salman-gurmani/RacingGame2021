using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = ("Level Data"))]
public class LevelData : ScriptableObject
{
    public enum LevelType { 
        
        SPRINT,
        LAP,
        TIMESPRINT    
    }
    public LevelType type = LevelType.SPRINT;

    public int laps;
    public float levelTime;
    public float aiCarMaxSpeed;
    public float aiCarTorque;

    [Range((int)2, (int)7)]
    public int sceneNum = 2;

    [Tooltip("Can be maximum 6")]
    public GameObject[] aiCars;
}
