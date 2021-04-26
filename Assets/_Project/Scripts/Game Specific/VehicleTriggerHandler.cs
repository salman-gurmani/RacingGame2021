using UnityEngine;

public class VehicleTriggerHandler : MonoBehaviour
{
    public float distanceBar;
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
        if (other.gameObject.tag == "Distance")
        {

            Toolbox.GameplayScript.distCalculation.points.Remove(other.gameObject.transform);
            Toolbox.GameplayScript.distCalculation.calculatingDistance();
            distanceBar = Toolbox.GameplayScript.distCalculation.mainDistance - Toolbox.GameplayScript.distCalculation.accumulateDistance;
        }
    }
}



