using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteListner : MonoBehaviour {

	public GameObject nextButton;
	public GameObject doubleRewardButton;
	public Text rewardTxt;
	public GameObject[] star;

	public int levelReward = 100;

    private void OnDestroy()
    {
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
    }

    private void Start()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.complete);

		Toolbox.GameManager.Analytics_LevelComplete();


		UnlockNextLevel();
		StarsHandling();
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

	//Handles stars and Reward
	private void StarsHandling()
	{
		if (Toolbox.GameplayScript.LevelCompleteTime > 40)
		{
			star[0].SetActive(true);
			levelReward *= 1;			
		}
		else if (Toolbox.GameplayScript.LevelCompleteTime > 20)
		{
			star[0].SetActive(true);
			star[1].SetActive(true);
			
			levelReward *= 2;
		}
		else {

			for (int i = 0; i < star.Length; i++)
			{
				star[i].SetActive(true);
			}

			levelReward *= 3;
		}

		rewardTxt.text = levelReward.ToString();
		Toolbox.DB.prefs.GoldCoins += levelReward;

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
}
