using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassengerHandler : MonoBehaviour
{
    public VehicleHandler vehicleHandler;

    private Transform targetPoint;
    public NavMeshAgent agent;
    public GameObject Ragdoll;
    public AudioClip DeadSfx;
    private Animator anim;
    private bool isEnterVehicle = false;
    public bool canDie = true;

    bool targetPointReached = false;

    private bool IsRightDoor = false;

    public float insideVehicleHeightOffset = -0.2f;

    public Transform TargetPoint { get => targetPoint; set => targetPoint = value; }
    public bool IsEnterVehicle { get => isEnterVehicle; set => isEnterVehicle = value; }

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (targetPoint) {

            if(agent.speed > 0 && agent.enabled)
                agent.SetDestination(targetPoint.position);

            //Debug.LogError("-> " + Vector3.Distance(this.transform.position, targetPoint.position));

            if (Vector3.Distance(this.transform.position, targetPoint.position) <= 0.3f)
            {
                this.transform.position = targetPoint.transform.position;
                this.transform.rotation = targetPoint.transform.rotation;

                if (IsEnterVehicle)
                {
                    agent.enabled = false;

                    EnterVehicle();
                    targetPointReached = true;

                    targetPoint = null;
                }
                else {

                    Idle();
                }
            }
        }        
    }


    public void Idle() {

        anim.SetInteger("Speed", 0);
        agent.speed = 0;

    }

    public void Walk()
    {

        anim.SetInteger("Speed", 1);
        agent.speed = 2;

        if (IsEnterVehicle)
        {
            Invoke("TimeReachPointCheck", 5);
        }
    }

    public void TimeReachPointCheck() {

        if (!targetPointReached) {

            this.transform.position = targetPoint.transform.position;
        }
    }

    public void EnterVehicle()
    {
        anim.SetTrigger("EnterVehicle");
        agent.speed = 0;

        //this.transform.rotation = targetPoint.transform.rotation;
        LeanTween.rotate(this.gameObject, targetPoint.transform.rotation.eulerAngles, 0.5f);
    }

    public void ExitVehicle()
    {
        anim.SetTrigger("ExitVehicle");
        agent.speed = 0;

        vehicleHandler.EnableDoorCamera(IsRightDoor);

    }

    public void SetDoor(bool _val) {

        IsRightDoor = _val;
        anim.SetBool("isRight", _val);
    }

    public void OpenVehicleDoor() {

        if(vehicleHandler)
            vehicleHandler.OpenDoor(IsRightDoor);
    }

    public void CloseVehicleDoor() {

        if (vehicleHandler)
            vehicleHandler.CloseDoor(IsRightDoor);

        if (IsEnterVehicle)
        {
            EnableVehicleReadyToGo();
        }
        else {

            Invoke("EnableVehicleReadyToGo", 1f);
        }

        
    }

    public void AdjustPlayerOffset() {

        if (IsEnterVehicle)
        {
            LeanTween.move(this.gameObject, new Vector3(this.transform.position.x, this.transform.position.y + insideVehicleHeightOffset, this.transform.position.z), 0.5f);
        }
        else
        {
            LeanTween.move(this.gameObject, new Vector3(this.transform.position.x, this.transform.position.y - insideVehicleHeightOffset, this.transform.position.z), 0.5f);
        }

        //LeanTween.move(this.gameObject, vehicleHandler.sitPoint.position, 0.5f);
    }

    public void Play_LetsGosfx()
    {
        int random = Random.Range(0, 3);
        if (this.gameObject.CompareTag("Female"))
        {
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Female[random]);
        }
        else if (this.gameObject.CompareTag("Male"))
        {
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Male[random]);
        }

    }

    public void EnableVehicleReadyToGo()
    {

        if (isEnterVehicle)
        {

            vehicleHandler.passengerInCarHandler = this;
            this.transform.SetParent(vehicleHandler.PedestrianParent.transform);
            vehicleHandler.EnableDriving();                    
            Toolbox.GameplayScript.RideStartHandling();
            Play_LetsGosfx();
        }
        else {

            vehicleHandler.passengerInCarHandler = null;
            this.transform.SetParent(null);
            agent.enabled = true;
            Walk();
            vehicleHandler.EnableDriving();

            Toolbox.GameplayScript.CompleteRideHandling();

        }
    }

    public void onDead()
    {
        if (!canDie)
            return;

        Instantiate(Ragdoll, transform.position, transform.rotation);
        Toolbox.Soundmanager.PlaySound(DeadSfx);
    }

}
