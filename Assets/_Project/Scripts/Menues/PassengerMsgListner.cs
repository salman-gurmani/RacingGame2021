
using UnityEngine;
using UnityEngine.UI;

public class PassengerMsgListner : MonoBehaviour
{
    public Text passengerName;
    public Text destination;
    public Text description;
    public Text timeLimit;
    public Text speedLimit;

    public void UpdateMsg(string _name, string _destination, string _description, float _timeLimit, float _speedLimit)
    {
        passengerName.text = _name;
        destination.text = _destination;
        description.text = _description;

        timeLimit.text = _timeLimit.ToString();
        speedLimit.text = _speedLimit.ToString();
    }

    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Toolbox.GameplayScript.PlayerObject.GetComponent<VehicleHandler>().EnableDriving();
        Destroy(this.gameObject);
    }

}
