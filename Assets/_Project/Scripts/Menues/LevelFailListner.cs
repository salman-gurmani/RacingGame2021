using UnityEngine;
using UnityEngine.UI;

public class LevelFailListner : MonoBehaviour {


	private void OnDestroy()
	{
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
	}
	private void Start()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.fail);

		Toolbox.GameManager.Analytics_LevelFail();

	}

	public void Press_SkipLevel()
	{

	}

	public void Press_Restart()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Game, true, 0);

		Destroy(this.gameObject);
	}

	public void Press_Home()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, true, 0);

		Destroy(this.gameObject);
	}
}
