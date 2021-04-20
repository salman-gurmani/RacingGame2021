using UnityEngine;

public class RCC_InputNew : MonoBehaviour {
    public RCC_CarControllerV3 rcc;
	// Use this for initialization
	void Start () {
        rcc = this.GetComponent<RCC_CarControllerV3>();
	}
	
	// Update is called once per frame
	void Update () {
       // rcc.gasInput = DrivingInput.accelValue;
       // rcc.brakeInput = (DrivingInput.brakeValue)?1:0;
       //rcc.steerInput = DrivingInput.steerValue;
       // rcc.handbrakeInput = (DrivingInput.handbrake)?1:0;


        rcc.gasInput = HUDListner.accelVal;
        rcc.brakeInput = HUDListner.brakeVal;
        rcc.steerInput = HUDListner.turnVal;
        rcc.handbrakeInput = HUDListner.handBrakeVal;

        if (Toolbox.HUDListner.canUseNOS) rcc.boostInput = HUDListner.nosVal;
        else rcc.boostInput = HUDListner.accelVal;
        //rcc.boostInput = (DrivingInput.nos) ? 1 : 0;

    }
}
