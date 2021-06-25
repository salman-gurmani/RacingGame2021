using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class VehicleTriggerHandler : MonoBehaviour
{
    public float distanceBar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            if(Toolbox.GameplayScript.levelsManager.CurLevelData.type != LevelData.LevelType.LAP)
                Toolbox.GameplayScript.RaceEndHandling();
        }

        if (other.gameObject.tag == "NOS")
        {
            other.GetComponent<CollectableHandler>().OnCollect();
            Toolbox.HUDListner.NosSlider.value = 100;
        }
        if (other.gameObject.tag == "CheckPoint")
        {
          //  Toolbox.HUDListner.TempTime += 20;
        }
        if (other.gameObject.tag == "Distance")
        {

            Toolbox.GameplayScript.levelsManager.CurLevelHandler.distScript.points.Remove(other.gameObject.transform);
            Toolbox.GameplayScript.levelsManager.CurLevelHandler.distScript.calculatingDistance();
            distanceBar = Toolbox.GameplayScript.levelsManager.CurLevelHandler.distScript.mainDistance - Toolbox.GameplayScript.levelsManager.CurLevelHandler.distScript.accumulateDistance;
        }
        if (other.gameObject.tag == "AICar")
        {
            other.gameObject.GetComponentInParent<CarController>().rotateWheel = true;

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AICar")
        {
            other.gameObject.GetComponentInParent<CarController>().rotateWheel = false;
        }
    }
}



