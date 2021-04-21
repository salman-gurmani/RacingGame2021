using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailListner : MonoBehaviour {

	public Text cashTxt;
	public Text remainingTime;
	public Text playerVehicleTxt;
	public int levelReward = 0;
	
	private int cashEarned = 0;

	private void OnDestroy()
	{
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
	}
	private void Start()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.fail);

		Toolbox.GameManager.Analytics_LevelFail();
		ShowRemainingTime();
		StatsHandling();
	}
	private void StatsHandling()
	{
		cashEarned = UnityEngine.Random.Range(100, 499);

		cashTxt.text = cashEarned.ToString();

		Toolbox.DB.prefs.GoldCoins += cashEarned;
		playerVehicleTxt.text = Toolbox.DB.prefs.LastSelectedVehicleName;
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

	public void ShowRemainingTime()
	{
		int roundedSec = Mathf.RoundToInt(Toolbox.GameplayScript.RemainingTime);
		int min = roundedSec / 60;
		int seconds = roundedSec - (min * 60);
		if (Toolbox.GameplayScript.RemainingTime <= 5) remainingTime.color = Color.red;
		remainingTime.text = String.Format("{0:D2} : {1:D2}", min, seconds);
	}
}
