using UnityEngine;


[CreateAssetMenu(fileName = "PlayerObjData", menuName = ("Player Data"))]
public class PlayerObjData : ScriptableObject
{
    public string name;
    public int price;

    [Range(0,1)]
    public float [] specs;

}
