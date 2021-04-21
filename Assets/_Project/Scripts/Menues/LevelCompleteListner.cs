using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelCompleteListner : MonoBehaviour {

	public GameObject nextButton;
	public GameObject doubleRewardButton;
	public Text rewardTxt, rankTxt, cashTxt;
	public Text remainingTime,playerVehicleTxt;
	public GameObject[] star;
	public int levelReward = 100;
	
	private int cashEarned = 100;

    private void OnDestroy()
    {
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
    }

    private void Start()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.complete);

		Toolbox.GameManager.Analytics_LevelComplete();

		if(Toolbox.DB.prefs.FuelTank < 8) Toolbox.DB.prefs.FuelTank += 1;
		UnlockNextLevel();
		StatsHandling();
		ShowRemainingTime();

	}

    private void UnlockNextLevel()
    {
		if (Toolbox.DB.prefs.LastSelectedLevel < Toolbox.DB.prefs.GameMode[Toolbox.DB.prefs.LastSelectedMode].GetLastUnlockedLevel())
			return;

		if (Toolbox.DB.prefs.LastSelectedLevel == Toolbox.DB.prefs.GameMode[Toolbox.DB.prefs.LastSelectedMode].LevelUnlocked.Length - 1)
		{
			//This is the last level of current mode
			nextButton.SetActive(false);
			Toolbox.DB.prefs.Mode2Unlocked = true;
		}
		else {

			Toolbox.DB.prefs.GameMode[Toolbox.DB.prefs.LastSelectedMode].LevelUnlocked[Toolbox.DB.prefs.LastSelectedLevel+1] = true;
		}
    }

	//Handles Stats, stars and Reward
	private void StatsHandling()
	{
		if (Toolbox.GameplayScript.LevelCompleteTime <= 10)
		{
			star[0].SetActive(true);
			levelReward *= 1;
			AssignRank(5, 20);
			cashEarned = UnityEngine.Random.Range(500, 1000);
		}
		else if (Toolbox.GameplayScript.LevelCompleteTime <= 15)
		{
			star[0].SetActive(true);
			star[1].SetActive(true);
			
			levelReward *= 2;
			AssignRank(20, 50);
			cashEarned = UnityEngine.Random.Range(1500, 2500);
		}
		else {

			for (int i = 0; i < star.Length; i++)
			{
				star[i].SetActive(true);
			}

			levelReward *= 3;
			AssignRank(50, 100);
			cashEarned = UnityEngine.Random.Range(2500, 4000);
		}

		rewardTxt.text = levelReward.ToString();
		cashTxt.text = cashEarned.ToString();
		playerVehicleTxt.text = Toolbox.DB.prefs.LastSelectedVehicleName;

		Toolbox.DB.prefs.GoldCoins += levelReward;
		Toolbox.DB.prefs.GoldCoins += cashEarned;

	}

	public void Press_Next()
	{
		if (Toolbox.DB.prefs.LastSelectedLevel < Toolbox.DB.prefs.GameMode[Toolbox.DB.prefs.LastSelectedMode].GetLastUnlockedLevel())
			Toolbox.DB.prefs.LastSelectedLevel++;

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Game, true, 0);

		Destroy(this.gameObject);
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

	public void Press_2XReward()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		AdsManager.instance.SetNShowRewardedAd(AdsManager.RewardType.DOUBLEREWARD, levelReward);

		doubleRewardButton.SetActive(false);
	}

	public void AssignRank(int min, int max)
    {
        int random = UnityEngine.Random.Range(min, max);
		rankTxt.text = "Rank " + random.ToString() +"%";
    }

	public void ShowRemainingTime()
    {
		int roundedSec = Mathf.RoundToInt(Toolbox.GameplayScript.LevelCompleteTime);
		int min = roundedSec / 60;
		int seconds = roundedSec - (min * 60);
		if (Toolbox.GameplayScript.LevelCompleteTime <= 5) remainingTime.color = Color.red;
		remainingTime.text = String.Format("{0:D2} : {1:D2}", min, seconds);
	}
}
