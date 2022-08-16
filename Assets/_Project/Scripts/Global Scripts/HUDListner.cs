using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDListner : MonoBehaviour {

    public enum ControlType {

        STEERING,
        ARROWS
    }
    public ControlType type = ControlType.STEERING;

    public Text timeTxt;
    
    public Text speedTxt;
    public Text playerSpeed;
    public Text paneltiesTxt;
    public Text paneltiesLimitTxt;
    public Text levelTxt;
    public Text lapTxt;
    public Text playerCarPosition;
    public Text pos;

    public GameObject uiParent;
    public GameObject controls;
    public GameObject steeringObj;
    public GameObject arrowsObj;
    public GameObject repairBtn;
    public GameObject centerText;
    public GameObject timeObj;
    public GameObject lapObj;
    public Toggle sportsBtn, cruiseBtn;
    public Button accelBtn, respawn;
    public Slider NosSlider;
    public Slider distanceSlider;
    public Image nosTank;

    public Button NosButton;


    public static float accelVal = 0;
    public static float brakeVal = 0;
    public static float handBrakeVal = 0;
    public static float turnVal = 0;

    public static float speed = 0;
    public RCC_CarControllerV3 carController;

    private float tempAccelnVal;
    private float tempTurnVal;
    private float accelDirection = 1;
    private float oldSpeed, newSpeedLimit;

    public float accelSwitchSpeed = 0.2f;
    public float steerSwitchSpeed = 0.2f;

    public bool isTimeSprintMode = false;

    private bool startTime = false;
    public bool outofTime = false;

    float tempTime = 0;

    private bool isAccPressed = false;

    public RCC_Inputs rccInputs;
    public static bool nosPressed = false;

    int paneltiesRecieved = 0;

    public bool StartTime { get => startTime; set => startTime = value; }
    public float TempTime { get => tempTime; set => tempTime = value; }
    public float centerTime;
    public int PaneltiesRecieved { get => paneltiesRecieved; set => paneltiesRecieved = value; }

    void Awake() {
        Toolbox.Set_HudListner(this.GetComponent<HUDListner>());
        rccInputs = RCC_InputManager.GetInputs();
    }

    public void EnableHud() {

        uiParent.SetActive(true);
    }

    public void DisableHUD() {
        
        ResetControls();
    }

    public void OnDestroy()
    {
        ResetControls();
    }

    private void Start()
    {
        accelVal = 0;
        brakeVal = 0;
        handBrakeVal = 0;
        turnVal = 0;

        UpdateControls();

        if(Toolbox.GameplayScript.PlayerObject)
            carController = Toolbox.GameplayScript.PlayerObject.GetComponent<RCC_CarControllerV3>();

        //RCC.SetController(1);
        
    }

    public void SetPlayerPos(string txt) {

        playerCarPosition.text = txt;
    }

    public void FillNos() {

        carController.useNOS = true;
        carController.NoS = 100;
    }

    private void Update()
    {
        //distanceSlider.maxValue = Toolbox.GameplayScript.levelsManager.CurLevelHandler.distScript.mainDistance;
        //distanceSlider.value = carController.GetComponent<VehicleTriggerHandler>().distanceBar;
        nosTank.fillAmount = carController.NoS / 100;// Mathf.MoveTowards(nosTank.fillAmount, carController.NoS / 100, 0.03f);

        if (nosTank.fillAmount > 0.05f)
        {
            NosButton.interactable = true;
        }
        else
        {
            NosButton.interactable = false;
        }

        accelVal = Mathf.MoveTowards(accelVal, tempAccelnVal, accelSwitchSpeed);
        
        //if (cruiseBtn.isOn) OnPress_Forward();
        switch (type)
        {

            case ControlType.STEERING:
                turnVal = SimpleInput.GetAxis("Horizontal");

                break;

            case ControlType.ARROWS:
                turnVal = Mathf.MoveTowards(turnVal, tempTurnVal, steerSwitchSpeed);

                break;
        }

        HandleTime();
       
        Handle_RCCInputs();

        //nosVal = Mathf.MoveTowards(nosVal, nosSwitchSpeed, 2.5f);
        //nosVal = Mathf.Clamp(tempNosVal * 2.5f, 1, nosSwitchSpeed);

        if (carController)
            playerSpeed.text = Mathf.RoundToInt(carController.speed).ToString();

        //if (carController.speed >= 20f && !isAccPressed) cruiseBtn.interactable = true;
        //else cruiseBtn.interactable = false;

#if UNITY_EDITOR

        HandlingKeyboardControls();
#endif

    }

    public void Handle_RCCInputs()
    {
        //rccInputs.throttleInput = accelVal;
        //rccInputs.steerInput = turnVal;
        //rccInputs.brakeInput = brakeVal;
        //rccInputs.boostInput = nosVal;
        //rccInputs.handbrakeInput = handBrakeVal;
    }

    public void SetLvlTxt(string _str) {

        levelTxt.text = _str;
    }

    public void SetLapTxt(int _str)
    {
        lapObj.SetActive(true);
        lapTxt.text = "Lap: " + _str.ToString() + "/" + Toolbox.GameplayScript.levelsManager.CurLevelData.laps;
    }


    private void HandlingKeyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnPress_Forward();
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            OnRelease_Forward();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnPress_Reverse();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            OnRelease_Reverse();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPress_HandBrake();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            OnRelease_Handbrake();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnPress_TurnLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnPress_TurnRight();
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            OnRelease_TurnRightOrLeft();
        }
    }

    public void SetTotalPanelties(int _val) {

        paneltiesLimitTxt.text = _val.ToString();
    }

    public void IncrementPanelty(int _val) {

        paneltiesRecieved += _val;
        paneltiesTxt.text = paneltiesRecieved.ToString();

    }

    private void HandleTime()
    {
        if (!isTimeSprintMode)
            return;


        if (StartTime) {
            if (tempTime <= 10 && tempTime > 5)
            {
                centerTime = tempTime;
                timeTxt.color = Color.yellow;
               // centerText.SetActive(true);
                //centerTime -= Time.deltaTime;
            }
            else if (tempTime <= 5) timeTxt.color = Color.red;
            TempTime -= Time.deltaTime;
            timeTxt.transform.parent.gameObject.SetActive(true);
            int roundedSec = Mathf.RoundToInt(TempTime);
            int min = roundedSec / 60;
            int seconds = roundedSec - (min * 60);

            timeTxt.text = String.Format("{0:D2} : {1:D2}", min, seconds);
            
            if (TempTime <= 0)
            {
                StartTime = false;
                outofTime = true;

                Toolbox.GameplayScript.RaceEndHandling();
            }            
        }
    }
    public void onPressNOS()
    {
        //carController.maxspeed = 400;
        //carController.speed += 100;

        nosPressed = true;
    }
    public void onReleaseNOS()
    {
    }

    public void SetNStartTime(float _val) {

        TempTime = _val;
        StartTime = true;
        isTimeSprintMode = true;

        timeObj.SetActive(true);
    }

    public void Press_Pause() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Toolbox.GameManager.Instantiate_PauseMenu();
    }

    public void Press_Camera()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Toolbox.GameplayScript.cameraScript.ChangeCamera();

    }
    public void Press_ControlChange()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        if (type == ControlType.ARROWS)
        {
            type = ControlType.STEERING;
            Toolbox.DB.prefs.IsSteerControl = true;
        }
        else {
            type = ControlType.ARROWS;
            Toolbox.DB.prefs.IsSteerControl = false;

        }

        UpdateControls();
    }

    public void UpdateControls() {

        if (Toolbox.DB.prefs.IsSteerControl)
            type = ControlType.STEERING;
        else
            type = ControlType.ARROWS;

        if (type == ControlType.ARROWS)
        {
            arrowsObj.SetActive(true);
            steeringObj.SetActive(false);
        }
        else
        {
            arrowsObj.SetActive(false);
            steeringObj.SetActive(true);
        }
    }

    public void Press_Settings()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Toolbox.GameManager.Instantiate_SettingsMenu();
    }

    public void OnPress_Horn() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.horn);
    }

    public void OnPress_ToggleGear() {

        accelDirection *= -1;

        //if (accelDirection > 0)
        //    accelBtn.sprite = accelImages[0];
        //else
        //    accelBtn.sprite = accelImages[1];
    }

    public void OnPress_Forward()
    {
        StartTime = true;
        tempAccelnVal = accelDirection;
        //rccInputs.throttleInput = tempAccelnVal;
    }
    //To disable Cruise Control on first click
    public void onClick_Forward()
    {
        isAccPressed = true;
    }
    public void OnRelease_Forward()
    {
        isAccPressed = false;
        tempAccelnVal = 0;
    }
    public void OnPress_Reverse()
    {
        StartTime = true;
        brakeVal = 1;
      //  RCC_Settings.instance.behaviorType = RCC_Settings.BehaviorType.Drift;
    }
    public void OnRelease_Reverse()
    {
        brakeVal = 0;
     //   RCC_Settings.instance.behaviorType = RCC_Settings.BehaviorType.Racing;
    }
    public void OnPress_TurnRight()
    {
        tempTurnVal = 1;
    }
    public void OnPress_TurnLeft()
    {
        tempTurnVal = -1;
    }
    public void OnRelease_TurnRightOrLeft()
    {
        tempTurnVal = 0;
    }
    public void OnPress_HandBrake()
    {
        StartTime = true;
        brakeVal = 1;
    }
    public void OnRelease_Handbrake()
    {
        brakeVal = 0;
    }
    public void OnPress_Reset() {

        //carController.ResetNew();
        respawn.interactable = false;
        Invoke("EnableRespawnbtn", 3f);
    }

    public void EnableRespawnbtn()
    {
        respawn.interactable = true;
    }

    public void Onpress_Repair()
    {
      //  RCC_Customization.Repair(carController);
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Repair);
    }

    public void OnPress_Turbo()
    {
      //  RCC_Customization.SetTurbo(carController, sportsBtn.isOn);
    }

    public void OnPress_Cruise()
    {
        if (cruiseBtn.isOn)
        {
            OnPress_Forward();
            oldSpeed = carController.maxspeed;
            newSpeedLimit = carController.speed;
           // RCC_Customization.SetMaximumSpeed(carController, newSpeedLimit);
        }
        else
        {
            Disable_CruiseControl();
            Debug.Log("Disable by CruiseBtn_itself");
        }

    }

    private void Disable_CruiseControl()
    {
        //RCC_Customization.SetMaximumSpeed(carController, oldSpeed);
        tempAccelnVal = 0;
    }

    public void ResetControls() {

        startTime = false;
        accelVal = 0;
        brakeVal = 0;
        handBrakeVal = 0;
        turnVal = 0;

        rccInputs.throttleInput = accelVal;
        rccInputs.steerInput = turnVal;
        rccInputs.brakeInput = brakeVal;
        rccInputs.boostInput = 0;
        rccInputs.handbrakeInput = handBrakeVal;

        uiParent.SetActive(false);
    }
}
