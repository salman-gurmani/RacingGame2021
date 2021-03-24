using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerDropoffHandler : MonoBehaviour
{
    public Transform passengerFinalPoint;
    public Transform specialEventDestination;

    public void SpecialEventHanlding() {

        this.transform.position = specialEventDestination.transform.position;

        Passenger pas = Toolbox.GameplayScript.levelsManager.CurLevelData.passenger[0];
        Toolbox.GameManager.InstantiateRide_Msg(pas.name, pas.destination_SE, pas.description_SE, pas.timeLimit_SE, pas.speedLimit_SE);

        Toolbox.HUDListner.SetNStartTime(pas.timeLimit_SE);
        Toolbox.HUDListner.SetSpeedLimit(pas.speedLimit_SE);

        Toolbox.GameplayScript.carArrowScript.Status(true);

        if (pas.startRaining)
            Toolbox.GameplayScript.RainStatus(true);

        Toolbox.GameplayScript.mapNavigator.SetTargetPoint(specialEventDestination.transform.position);

    }
}



