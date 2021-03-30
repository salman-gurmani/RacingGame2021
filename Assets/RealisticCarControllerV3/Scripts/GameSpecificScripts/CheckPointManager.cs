using UnityEngine;

public class CheckPointManager : MonoBehaviour {

	// Use this for initialization
   
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CancelButton()
    {
        this.gameObject.SetActive(false);

    }
    public void StartButton()
    {

        //GameManager.Instance.gameplayScript.StartCheckPointRace();
        Time.timeScale = 1;
        this.gameObject.SetActive(false);

    }
}
