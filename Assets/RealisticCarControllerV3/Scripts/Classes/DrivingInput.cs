using UnityEngine;

public class DrivingInput : MonoBehaviour {
    public static float accelValue = 0.0f;
    public static bool brakeValue = false;
    public static float reverseValue = 0.0f;
    public static float steerValue = 0.0f;
    public static bool handbrake = false;
    public static bool nos = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
#if MOBILE_INPUT





#else

        KeyboardController();

#endif

    }
    public void KeyboardController()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
           // Debug.Log("Coming Here");
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
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, -1, 0.05f);

            Debug.Log(DrivingInput.steerValue);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.05f);
            Debug.Log(DrivingInput.steerValue);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 1, 0.05f);

        }


        if (Input.GetKeyUp(KeyCode.D))
        {
            DrivingInput.steerValue = Mathf.MoveTowards(DrivingInput.steerValue, 0, 0.05f);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrivingInput.handbrake = true;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DrivingInput.handbrake = false;

        }



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DrivingInput.nos = true;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DrivingInput.nos = false;

        }
    }
}
