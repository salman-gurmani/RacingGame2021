using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardCam : MonoBehaviour
{
    Transform cam;

    private void Start()
    {
        cam = Toolbox.GameplayScript.camListner.transform;
    }

    void Update()
    {
        this.transform.LookAt(cam);
    }
}
