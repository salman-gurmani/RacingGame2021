using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPickupHandler : MonoBehaviour
{
    [Tooltip("Position at which this pedestrian system should be enabled")]
    public int passengerId = 0;

    [Tooltip("If Enabled = PassengerPrefab will be ignored and prefab will be loaded from LevelData")]
    public bool getPassengerFromLevelData;

    public GameObject passengerPrefab;
    public Transform passengerWaitPoint;

    private PassengerHandler passenger;

    private VehicleHandler vehicleHandler;

    public GameObject dropOffPoint;

    public VehicleHandler VehicleHandler { get => vehicleHandler; set => vehicleHandler = value; }



    private void Start()
    {
        if (getPassengerFromLevelData)
            passengerPrefab = Toolbox.GameplayScript.levelsManager.CurLevelData.passenger[passengerId].prefab;

        dropOffPoint.SetActive(false);

        if(Toolbox.GameplayScript.carArrowScript)
            Toolbox.GameplayScript.carArrowScript.SetTarget(this.transform);

        //Toolbox.GameplayScript.mapNavigator.SetTargetPoint(this.gameObject.transform.position);

        InitPassenger();
    }

    public void SetVehicle(VehicleHandler _vehicle) {

        vehicleHandler = _vehicle;
    }

    void InitPassenger() {

        passenger = Instantiate(passengerPrefab, passengerWaitPoint.position, passengerWaitPoint.rotation).GetComponent<PassengerHandler>();

    }

    public void MovePassengerToVehicleDoor() {

        passenger.canDie = false;

        //Right Door is Close
        if (Vector3.Distance(passenger.transform.position, vehicleHandler.rightDoorStandPoint.position) < Vector3.Distance(passenger.transform.position, vehicleHandler.leftDoorStandPoint.position)) {

            passenger.TargetPoint = vehicleHandler.rightDoorStandPoint.transform;
            passenger.SetDoor(true);

            vehicleHandler.EnableDoorCamera(true);
        }
        else { //Left Door is close

            passenger.TargetPoint = vehicleHandler.leftDoorStandPoint.transform;
            passenger.SetDoor(false);

            vehicleHandler.EnableDoorCamera(false);
        }

        passenger.vehicleHandler = vehicleHandler;
        passenger.IsEnterVehicle = true;
        passenger.Walk();

        dropOffPoint.SetActive(true);

        Toolbox.GameplayScript.carArrowScript.SetTarget(dropOffPoint.transform);

        Toolbox.GameplayScript.mapNavigator.SetTargetPoint(dropOffPoint.transform.position);

    }

    public void EnableVehicleReadyToGo() {

        vehicleHandler.passengerInCarHandler = passenger;
        
        passenger.transform.SetParent(VehicleHandler.PedestrianParent.transform);

        vehicleHandler.EnableDriving();
    }


}
