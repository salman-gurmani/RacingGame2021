using System;
using UnityEngine;

public class HudMenuRacing : MonoBehaviour {

    // Use this for initialization
    private bool pauseMenuToggle = false;
    public ControlButtons controlButton;
    public GameObject raceStartMenu;
    public Mission thisMission;
    private bool leftTrue = false;
    private bool rightTrue = false;
    bool tiltControl;
    private float sensitivity { get { return RCC_Settings.Instance.UIButtonSensitivity; } }
    private float gravity { get { return RCC_Settings.Instance.UIButtonGravity; } }
    void Start() {
        SetControls();
    }
    public void SetControls()
    {
        controlButton.accelButton.SetActive(false); 
        controlButton.brakeButton.SetActive(false); 
        controlButton.brakeButton1.SetActive(false); 
        controlButton.HandBrakeButton.SetActive(false); 
        controlButton.HandBrakeButton1.SetActive(false); 
        controlButton.leftButton.SetActive(false); 
        controlButton.rightButton.SetActive(false);
        controlButton.steeringWheel.SetActive(false);


        //#if MOBILE_INPUT
        // if (UserPrefs.currentControl == 1)//tilt
        //{
        //    tiltControl = true;
        //    controlButton.accelButton.SetActive(true);
        //    controlButton.HandBrakeButton1.SetActive(true);
        //    controlButton.brakeButton1.SetActive(true);
        //}
        //else if (UserPrefs.currentControl == 2)//steering
        //{
        //    controlButton.accelButton.SetActive(true);
        //    controlButton.HandBrakeButton.SetActive(true);
        //    controlButton.brakeButton.SetActive(true);
        //    controlButton.steeringWheel.SetActive(true);
        //    tiltControl = false;
        //}
        //else if (UserPrefs.currentControl==3)
        //{
        //    controlButton.accelButton.SetActive(true);
        //    controlButton.HandBrakeButton.SetActive(true);
        //    controlButton.brakeButton.SetActive(true);
        //    controlButton.leftButton.SetActive(true);
        //    controlButton.rightButton.SetActive(true);
        //    tiltControl = false;
        //}
        //  #endif

       
            controlButton.accelButton.SetActive(true);
            controlButton.HandBrakeButton.SetActive(true);
            controlButton.brakeButton.SetActive(true);
            controlButton.leftButton.SetActive(true);
            controlButton.rightButton.SetActive(true);
            tiltControl = false;
        

    }
    // Update is called once per frame
    void Update() {
        if (tiltControl)
        {
            DrivingInput.steerValue = Input.acceleration.x;
        }
        else
        {

#if MOBILE_INPUT

            
            if (leftTrue)
            {
                DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, -1, 0.1f);
            }
            else if (rightTrue)
            {

                DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 1, 0.1f);
            }
            else
            {
                DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.4f);
            }

          

#endif




        }


#if MOBILE_INPUT





#else
      
        KeyboardController();

#endif



    }
    //RCC Controller
    public  void KeyboardController()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Coming Here");
            DrivingInput.accelValue = 1;

        }
         
        if (Input.GetKeyUp(KeyCode.W))
            {
              
                DrivingInput.accelValue = 0;

            }

         if (Input.GetKeyDown(KeyCode.S))
        {
            DrivingInput.accelValue = -1;
            DrivingInput.brakeValue = true;

        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            DrivingInput.accelValue = 0;
            DrivingInput.brakeValue = false;

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, -1, 0.7f);

        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.7f);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 1, 0.7f);

        }


        if(Input.GetKeyUp(KeyCode.D))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.7f);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrivingInput.handbrake = true;

        }
         if(Input.GetKeyUp(KeyCode.Space))
        {
            DrivingInput.handbrake = false;

        }
       
    }



    
    
    public void OnPressAccel()
    {
        DrivingInput.accelValue = 1;


    }
    public void OnReleaseAccel()
    {

        DrivingInput.accelValue = 0;

    }
    public void OnPressBrake()
    {
        DrivingInput.accelValue = -1;
        DrivingInput.brakeValue = true;
    }
    public void OnReleaseBrake()
    {
        DrivingInput.brakeValue = false;
        DrivingInput.accelValue = 0;
    }
    public void OnPressHandBrake()
    {
        DrivingInput.handbrake = true;
    }
    public void OnReleaseHandBrake()
    {
        DrivingInput.handbrake = false;

    }
   public void OnPressNos()
    {
        DrivingInput.nos = true;

    }
    public void OnReleaseNos()
    {
        DrivingInput.nos = false;
    }
    public void OnPressRight()
    {


        this.rightTrue = true;
       // DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 1, 0.03f);

    }
    public void OnReleaseRight()
    {
        this.rightTrue = false;
       // DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.3f);
    }
    public void OnPressLeft()
    {
        this.leftTrue = true;
       // DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, -1, 0.3f);

    }
    public void OnReleaseLeft()
    {

        this.leftTrue = false;
       // DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.3f);
    }
    public void PauseButton()
    {
       // GameManager.Instance.ChangeState(GameManager.GameStateSubMenu.PauseMenu);
        
            

    }
    public void  HideAndDisable()
    {
        this.GetComponent<UIPanel>().alpha = 0;
        UIButton[] hudButtons = this.GetComponentsInChildren<UIButton>();
        foreach(UIButton button in hudButtons)
        {
            button.isEnabled = false;
        }
    }
    public void ShowAndEnable()
    {
        this.GetComponent<UIPanel>().alpha = 1;
        UIButton[] hudButtons = this.GetComponentsInChildren<UIButton>();
        foreach (UIButton button in hudButtons)
        {
            button.isEnabled = true;
        }
    }

    [System.Serializable]
    public class ControlButtons{
        public GameObject accelButton;
        public GameObject brakeButton;
        public GameObject brakeButton1;
        public GameObject HandBrakeButton;
        public GameObject HandBrakeButton1;
        public GameObject leftButton;
        public GameObject rightButton;
        public GameObject steeringWheel;
        }
    [System.Serializable]
    public class Mission
    {

        public UILabel currentMission;
        public UILabel totalMission;
    }
}
