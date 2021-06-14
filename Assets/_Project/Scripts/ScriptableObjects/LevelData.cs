using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = ("Level Data"))]
public class LevelData : ScriptableObject
{
    public float levelTime;
    public float carPower;

    [Tooltip("Can be maximum 6")]
    public GameObject[] aiCars;
}
