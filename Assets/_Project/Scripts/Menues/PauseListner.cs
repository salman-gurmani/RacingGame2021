using UnityEngine;

public class PauseListner : MonoBehaviour {

	void OnDestroy(){
		Toolbox.Soundmanager.UnPause_All ();
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
		Time.timeScale = 1;
		AudioListener.pause = false;
	}
    private void OnEnable()
    {
		Toolbox.Soundmanager.Pause_All();
		AudioListener.pause = true;
	}
    private void Start()
    {
		Time.timeScale = 0;
	}

    void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Press_Play ();
		}
	}

	public void Press_Play(){
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameplayScript.EnableHud ();

		Destroy(this.gameObject);
	}
	
	public void Press_Restart()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Game, true, 0);
		Toolbox.GameplayScript.stopmap = true;

		Destroy(this.gameObject);
	}

	public void Press_Home(){

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, true, 0);
		Toolbox.GameplayScript.stopmap = true;

		Destroy(this.gameObject);
	}

}
