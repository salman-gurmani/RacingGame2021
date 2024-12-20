﻿using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 
/// Every Menu will normally have:
/// Farward button : to next screen
/// Backward button : to previous screen
/// 
/// </summary>

public class MenuHandler : MonoBehaviour {


	int currentMenuNum = 0;

	public GameObject clone;
	public GameObject[] menuList;
	private DateTime lastRewardTime;

	void Awake(){

		Toolbox.Set_MenuHandler(this);

        AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);

		if (Toolbox.GameManager.directShowLevelSel)
		{
			menuList[0].SetActive(false);
			menuList[2].SetActive(true);

			currentMenuNum = 2;
			Toolbox.GameManager.directShowLevelSel = false;
		}
		else { 
			
			StartMainMenu();
		}

	}

	void StartMainMenu(){
	
		if (menuList [0]) {

			//clone = (GameObject) Instantiate(menuList[0]);
			//clone.transform.parent = this.transform;

			menuList[0].SetActive(true);

			currentMenuNum = 0;
			
		} else {
		
			Debug.LogError ("MainMenu is not initialized in MenuHandler");
		}
			
	}

    private void Start()
    {
		//Toolbox.Soundmanager.Play_MenuBGSound();
		lastRewardTime = Toolbox.DB.prefs.LastClaimedRewardTime;
		if (DateTime.Now >= lastRewardTime.AddHours(24))
			Toolbox.GameManager.Instantiate_DailyRewardMenu();
	}

    public void Show_NextUI(){

		if (currentMenuNum + 1 < menuList.Length) {

			//GameObject tempClone = clone;

			//clone = (GameObject) Instantiate(menuList[currentMenuNum + 1]);
			//clone.transform.parent = this.transform;

			menuList[currentMenuNum].SetActive(false);
			menuList[currentMenuNum + 1].SetActive(true);

			//Destroy(tempClone);

			currentMenuNum++;
					
		} else {

			Debug.Log ("Menu List has reached at last menu. Loading GameScene!");

			//ToggleGraphics ();

			//Destroy (this.clone);

			//			Debug.LogError ("SCENE = " + Toolbox.Instance.userPrefs.currentLevel);
			Toolbox.Soundmanager.Stop_PlayingBGSound();
			//Toolbox.GameManager.LoadScene(Constants.sceneIndex_Game, true, 0);
			Toolbox.GameManager.LoadScene(Toolbox.GameManager.GetLastSelectedLevelSceneIndex(), true, 0);

		}

	}

	public void Show_PrevUI(){

		if (currentMenuNum - 1 >= 0) {

			//GameObject tempClone = clone;

			//clone = (GameObject) Instantiate(menuList[currentMenuNum - 1]);
			//clone.transform.parent = this.transform;


			menuList[currentMenuNum].SetActive(false);
			menuList[currentMenuNum - 1].SetActive(true);

			//Destroy (tempClone);

			currentMenuNum--;

		} else {

			Debug.Log ("Menu List has reached at First menu. This is the limit!");
		}

	}

}
