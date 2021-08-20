using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
public class PauseListner : MonoBehaviour {
	public GameObject tiltBtn;
	public Slider tiltSlider;
	
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
		tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;
		tiltSlider.value = Toolbox.DB.prefs.TiltSensitivity;
	}

    void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Press_Play ();
		}
	}

	public void Press_Play(){
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.HUDListner.EnableHud();

		Destroy(this.gameObject);
	}
	
	public void Press_Restart()
	{
		Toolbox.GameplayScript.cameraScript.enabled = false;
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Toolbox.GameManager.LoadScene(SceneManager.GetActiveScene().buildIndex, true, 0);

		Toolbox.GameplayScript.stopmap = true;
		Destroy(this.gameObject);
	}

	public void Press_Home(){

		Toolbox.GameplayScript.cameraScript.enabled = false;
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, true, 0);
		Toolbox.GameplayScript.stopmap = true;

		Destroy(this.gameObject);
	}
	public void GyroToggel()
	{
		Toolbox.DB.prefs.TiltControl = !Toolbox.DB.prefs.TiltControl;
		if (Toolbox.DB.prefs.TiltControl) RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
		else RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);

		tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;
	}

	public void Set_GyroSensitivity()
	{
		Toolbox.DB.prefs.TiltSensitivity = tiltSlider.value;
		RCC_Settings.Instance.gyroSensitivity = tiltSlider.value;
	}





}
