using UnityEngine;

public class VehicleTriggerHandler : MonoBehaviour
{
    private bool levelEnded = false;
    public bool isDropped = false;
    private VehicleHandler vehicleHandler;

    float accidentTimer = 0;
    bool specialScenerioImplemented = false;
    bool isPicked = false;
    private GameObject temp;
    
    private void Start()
    {
        vehicleHandler = this.GetComponent<VehicleHandler>();
    }

    private void Update()
    {
        HandleAccidentResetTime();
    }

    void HandleAccidentResetTime() {

        if (accidentTimer >= 0)
        {
            accidentTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PickPoint") && !isPicked) {

            if (GetComponent<RCC_CarControllerV3>().speed <= 40)
            {
                StopCarForPassengerPickup(other.GetComponent<PassengerPickupHandler>());
                other.gameObject.SetActive(false);
                Toolbox.GameplayScript.carArrowScript.Status(false);
                isPicked = true;
            }
        }

        if (other.CompareTag("DropPoint"))
        {
            temp = other.gameObject;
            //if passenger is in car
            if (vehicleHandler.passengerInCarHandler && 
                GetComponent<RCC_CarControllerV3>().speed <= 40 &&
                !isDropped)
            { 
                Toolbox.GameplayScript.carArrowScript.Status(false);

                StopCarForPassengerDropof(other.GetComponent<PassengerDropoffHandler>());
                temp.GetComponent<BoxCollider>().enabled = false;
                Invoke("TurnOnCollider", 5f);
            }
        }
    }
    void TurnOnCollider()
    {
        temp.GetComponent<BoxCollider>().enabled = true;
        //Debug.Log("Box Collider Enabled");
    }
    private void OnTriggerEnter(Collider other) { 

        if (other.CompareTag("Pedestrian")) {

            Toolbox.GameManager.Instantiate_PaneltyMsg("Passenger Killed!");
            other.gameObject.GetComponent<PedestrianSystem.Pedestrian>().Dead();
        }
        if (other.CompareTag("PickPoint"))
        {
            if (other.GetComponent<PassengerPickupHandler>().passengerPrefab.CompareTag("Female"))
            {
                Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.F_TaxiCall);
            }
            else Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.M_TaxiCall);

        }

        if (other.CompareTag("CarWash"))
        {
           Toolbox.HUDListner.repairBtn.SetActive(true);
            other.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CarWash"))
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            Toolbox.HUDListner.repairBtn.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle") && !levelEnded)
        {

            LevelEndHandling();
            Toolbox.GameManager.Instantiate_LevelFail(0.5f);

            if (collision.collider.GetComponentInParent<Animator>())
                collision.collider.GetComponentInParent<Animator>().enabled = false;

        }
        else if (collision.collider.CompareTag("Ragdoll"))
        {

        }
        else if (collision.collider.CompareTag("Roads"))
        {

        }else if (collision.collider.CompareTag("Male") || collision.collider.CompareTag("Female"))
        {
            collision.gameObject.GetComponent<PassengerHandler>().onDead();
            collision.gameObject.SetActive(false);
            LevelEndHandling();
            Toolbox.GameplayScript.LevelFailHandling();
        }
        else if (collision.collider.CompareTag("FireHydrant"))
        {
            collision.gameObject.transform.parent.GetChild(1).gameObject.SetActive(true);
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.WaterExplosion);
            Toolbox.HUDListner.cruiseBtn.isOn = false;
            if (accidentTimer <= 0)
            {
                if (vehicleHandler.passengerInCarHandler != null) Passenger_shout();
                Toolbox.GameManager.Instantiate_PaneltyMsg("Accident Panelty!");

                accidentTimer += 5;
            }
        }
        else
        {
            Toolbox.HUDListner.cruiseBtn.isOn = false;
            if (accidentTimer <= 0)
            {
                if (vehicleHandler.passengerInCarHandler != null) Passenger_shout();
                Toolbox.GameManager.Instantiate_PaneltyMsg("Accident Panelty!");
                
                accidentTimer += 5;
            }
        }

        //Debug.Log("Hit = " + collision.collider.name);
        //Debug.Log("Hit Tag = " + collision.collider.tag);
    }

    public void Passenger_shout()
    {
        int random = Random.Range(0, 5);
        //Debug.Log("Passenegr Shouted: " + random);
        if (vehicleHandler.passengerInCarHandler.CompareTag("Male"))
        {
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Male_Shouting[random]);
        }
        else if (vehicleHandler.passengerInCarHandler.CompareTag("Female"))
        {
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Female_Shouting[random]);
        }
    }

    private void LevelEndHandling() {

        levelEnded = true;
        Toolbox.HUDListner.OnRelease_Forward();
        Toolbox.HUDListner.OnPress_Reverse(); 
        Toolbox.HUDListner.DisableHUD();

        //Toolbox.GameplayScript.DisableHud();
    }

    void StopCarForPassengerPickup(PassengerPickupHandler _handler) {

        this.GetComponent<VehicleHandler>().DisableDriving();

        _handler.SetVehicle(this.GetComponent<VehicleHandler>());
        _handler.MovePassengerToVehicleDoor();
    }

    void StopCarForPassengerDropof(PassengerDropoffHandler _handler)
    {
        isDropped = true;
        if (Toolbox.GameplayScript.levelsManager.CurLevelData.passenger[0].hasSpecialEvent && !specialScenerioImplemented)
        {
            vehicleHandler.DisableDriving();
            _handler.gameObject.SetActive(true);
            _handler.SpecialEventHanlding();
            specialScenerioImplemented = true;
            isDropped = false;
            Toolbox.GameplayScript.isSE_implemented = specialScenerioImplemented;
            //Toolbox.HUDListner.OnPress_Cruise();
        }
        else {
            Invoke("PlayThankyouSfx", 2f);
            _handler.gameObject.SetActive(false);
            vehicleHandler.DisableDriving();
            vehicleHandler.passengerInCarHandler.ExitVehicle();
            vehicleHandler.passengerInCarHandler.TargetPoint = _handler.passengerFinalPoint;
            vehicleHandler.passengerInCarHandler.IsEnterVehicle = false;
        }
    }
    public void PlayThankyouSfx()
    {
        if (vehicleHandler.passengerInCarHandler.CompareTag("Male")) Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.ThankYou_M);
        else Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.ThankYou_F);
    }
}



