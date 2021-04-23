using UnityEngine;

public class VehicleTriggerHandler : MonoBehaviour
{
    public int current;
    public GameObject hitObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NOS")
        {
            Toolbox.HUDListner.NosSlider.value = 100;
        }
        if (other.gameObject.tag == "Point")
        {
            current += 1;
            hitObject = other.gameObject;
        }
    }
}



