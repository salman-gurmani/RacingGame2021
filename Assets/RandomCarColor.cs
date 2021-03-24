using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCarColor : MonoBehaviour
{
    public MeshRenderer CarBody;
    public Material [] BodyMat;
    public void OnEnable()
    {
        Apply_RandomMat();
    }

    public void Apply_RandomMat()
    {
        int random = Random.Range(0, BodyMat.Length-1);
        CarBody.material = BodyMat[random];
    }
    public void Apply_RandomColor()
    {
        //BodyMat.color = new Color(Random.value, Random.value, Random.value);
    }
}
