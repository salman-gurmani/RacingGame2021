using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public GameObject[] passengerPoints;
    public int curPassengerPoint = 0;

    private void Start()
    {
        curPassengerPoint = 0;

        EnablePassengerPoint(0);
    }

    public void EnablePassengerPoint(int _val) {

        if (_val >= passengerPoints.Length)
            return;

        passengerPoints[curPassengerPoint].SetActive(false);
        passengerPoints[_val].SetActive(true);

        curPassengerPoint = _val;
    }
}
