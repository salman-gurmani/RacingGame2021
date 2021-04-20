using UnityEngine;

public class VehicleTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NOS")
        {
            Toolbox.HUDListner.NosSlider.value = 100;
        }
    }
}



