using UnityEngine;

public class VehicleTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NOS")
        {
            Toolbox.HUDListner.NosSlider.value = 100;
        }
        if (other.gameObject.tag == "CheckPoint")
        {
            Toolbox.HUDListner.TempTime += 20;
        }
    }
}



