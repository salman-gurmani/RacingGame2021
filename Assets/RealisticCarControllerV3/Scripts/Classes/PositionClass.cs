using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionClass 
{
    public GameObject position;
    public bool check = false;
    public PositionClass(GameObject go,bool isCrossed)
    {
        this.position = go;
        this.check = isCrossed;
    }
}
