using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareFix : MonoBehaviour
{
    public GameObject cameraMain;
    public float BrightnessMultiplier = 4.0f;
    public float RelativeAngle;
    public float MinHeadlightAngle = -170.0f;
    public float MaxHeadlightAngle = 9.0f;
    public float distance;
    LensFlare headlightFlare;
    public float brightness;


    void Start()
    {
        if (cameraMain == null) cameraMain = GameObject.FindWithTag("MainCamera");
        headlightFlare = transform.GetComponent<LensFlare>();
    }

    void Update()
    {
        distance = Mathf.Round(Vector3.Distance(cameraMain.transform.position, this.transform.position));
        brightness = BrightnessMultiplier / distance;

        // get positive or negative angle between cameraToHeadlight vector and headlight forward facing direction
        Vector3 Cam2Headlight = transform.position - cameraMain.transform.position;
        Vector3 referenceForward = transform.forward;
        Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);
        Vector3 newDirection = Cam2Headlight;
        float angle = Vector3.Angle(newDirection, referenceForward);
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));

        RelativeAngle = Mathf.Round(sign * angle);

        // show/hide lens flare depending on camera's angle relative to headlight      
        if (RelativeAngle > MinHeadlightAngle && RelativeAngle < MaxHeadlightAngle)
        {
            // if visible, adjust flare brightness depending on camera's distance to car
            if (brightness < 2 && brightness > 0.8) headlightFlare.brightness = brightness;
        }
        //if not visible set brightness to 0
        else
        {
            headlightFlare.brightness = 0;

        }

    }
}
