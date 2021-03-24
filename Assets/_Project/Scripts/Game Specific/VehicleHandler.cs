using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHandler : MonoBehaviour
{
    public Animator anim;
    
    public PassengerHandler passengerInCarHandler;

    public Transform rightDoorStandPoint;
    public Transform leftDoorStandPoint;

    public Transform [] rightDoorCamPoint;
    public Transform [] leftDoorCamPoint;
    public GameObject doorViewCamera;

    public Transform sitPoint;

    [SerializeField] private Transform pedestrianParent;

    Rigidbody rigidbody;

    public Transform PedestrianParent { get => pedestrianParent; set => pedestrianParent = value; }

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    public void OpenDoor(bool isRight) {

        if (isRight) {

            anim.SetTrigger("right_open");
        }
        else { 
        
            anim.SetTrigger("left_open");
        }
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.DoorOpen);
    }

    public void CloseDoor(bool isRight)
    {
        if (isRight)
        {
            anim.SetTrigger("right_close");
        }
        else
        {
            anim.SetTrigger("left_close");
        }
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.DoorClose);
    }

    public void DisableDriving() {

        rigidbody.isKinematic = true;
        HUDListner.handBrakeVal = 1;

        Toolbox.HUDListner.OnRelease_Forward();
        Toolbox.HUDListner.OnRelease_Reverse();
        Toolbox.HUDListner.OnRelease_TurnRightOrLeft();

        Toolbox.HUDListner.controls.SetActive(false);
        Toolbox.HUDListner.respawn.gameObject.SetActive(false);
    }

    public void EnableDriving() {

        rigidbody.isKinematic = false;
        HUDListner.handBrakeVal = 0;

        Toolbox.HUDListner.OnRelease_Forward();
        Toolbox.HUDListner.OnRelease_Reverse();
        Toolbox.HUDListner.OnRelease_TurnRightOrLeft();
        Toolbox.HUDListner.respawn.gameObject.SetActive(true);
        Toolbox.HUDListner.controls.SetActive(true);

        DisableDoorCamera();
    }

    public void EnableDoorCamera(bool isRight) {

        Toolbox.GameManager.Instantiate_Blackout();
        //new
        Toolbox.GameplayScript.camListner.GetComponent<Camera>().enabled = false;
        doorViewCamera.SetActive(true);

        if (isRight)
        {
            int view = Random.Range(0, rightDoorCamPoint.Length);
            doorViewCamera.transform.position = rightDoorCamPoint[view].position;
            doorViewCamera.transform.rotation = rightDoorCamPoint[view].rotation;
        }
        else {

            int view = Random.Range(0, leftDoorCamPoint.Length);
            doorViewCamera.transform.position = leftDoorCamPoint[view].position;
            doorViewCamera.transform.rotation = leftDoorCamPoint[view].rotation;
        }

    }

    public void DisableDoorCamera() {

        doorViewCamera.SetActive(false);

        Toolbox.GameplayScript.camListner.GetComponent<Camera>().enabled = true;
    }
}
