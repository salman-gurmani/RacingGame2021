using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class DailyReward : MonoBehaviour
{
    public int[] EachDayReward = new int[7];
    public GameObject[] RewardBoxes;
    public Button CollectableBtn;
    public GameObject claimedTick, glowObj;
    public RuntimeAnimatorController Controller;

    private DateTime lastRewardTime;
    private int currentDay = 0;
    private int reward;
    
    void OnEnable()
    {
        lastRewardTime = Toolbox.DB.prefs.LastClaimedRewardTime;
        currentDay = Toolbox.DB.prefs.RewardDay;
        CheckRewardEligibilty();
        AssignEverything();
    }

    public void CheckRewardEligibilty()
    {
        Debug.Log("RewardDay: " + currentDay+1);
        UIShiny _object = RewardBoxes[currentDay].gameObject.AddComponent<UIShiny>();
        _object.effectPlayer.loop = true;
        _object.effectPlayer.duration = 2.7f;
        _object.effectPlayer.play = true;
        if (currentDay== 0 && PlayerPrefs.GetInt("FirstTimeClaim") == 0)
        {
            Toolbox.DB.prefs.LastClaimedRewardTime = DateTime.Now;
            Debug.Log("First Time Reward: You have been rewarded 10 coins");
            //currentDay++;
            Toolbox.DB.prefs.RewardDay = currentDay;
            reward = EachDayReward[0];
            PlayerPrefs.SetInt("FirstTimeClaim", 1);
            CollectableBtn.interactable = true;
        }
        else if (currentDay<=6 && DateTime.Now >= lastRewardTime.AddHours(24))
        {
            switch (currentDay)
            {
                case 0:
                    Debug.Log("You have been rewarded 10 coins");
                    reward = EachDayReward[0];
                    //currentDay++;
                    break; 
                case 1:
                    Debug.Log("You have been rewarded 20 coins");
                    reward = EachDayReward[1];
                    //currentDay++;
                    break;
                case 2:
                    Debug.Log("You have been rewarded 30 coins");
                    reward = EachDayReward[2];
                    //currentDay++;
                    break;
                case 3:
                    Debug.Log("You have been rewarded 40 coins");
                    reward = EachDayReward[3];
                    //currentDay++;
                    break;
                case 4:
                    Debug.Log("You have been rewarded 50 coins");
                    reward = EachDayReward[4];
                    //currentDay++;
                    break;
                case 5:
                    Debug.Log("You have been rewarded 60 coins");
                    reward = EachDayReward[5];
                    //currentDay++;
                    break;
                case 6:
                    Debug.Log("You have been rewarded 70 coins");
                    reward = EachDayReward[6];
                    //currentDay =0;
                    break;
            }
            
            CollectableBtn.interactable = true;
        }
    }

    public void CollectReward()
    {
        CollectableBtn.interactable = false;
        Toolbox.DB.prefs.GoldCoins += reward;
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.CollectReward);
        Debug.Log("Gold Collected");
        Toolbox.GameManager.Instantiate_CoinsEffect();
        //claimedTick.transform.SetParent(RewardBoxes[currentDay].transform);
        //claimedTick.GetComponent<RectTransform>().position = RewardBoxes[currentDay].transform.position;

        glowObj.SetActive(false);
        GameObject _claimTick = RewardBoxes[currentDay].transform.GetChild(3).gameObject;
        _claimTick.AddComponent<Animator>();
        _claimTick.GetComponent<Animator>().runtimeAnimatorController = Controller;
        _claimTick.SetActive(true);
        RewardBoxes[currentDay].GetComponent<UIShiny>().Stop(true);
        Toolbox.MenuHandler.menuList[0].GetComponent<MainMenuListner>().UpdateTxt();
        
        if (currentDay < 6) currentDay++;
        else currentDay = 0;
        Toolbox.DB.prefs.LastClaimedRewardTime = DateTime.Now;
        Toolbox.DB.prefs.RewardDay = currentDay;
    }

   private void AssignEverything()
    {
        for (int i=0; i< EachDayReward.Length; i++) //for CoinsText
        {
            RewardBoxes[i].transform.GetChild(2).GetComponent<Text>().text = EachDayReward[i].ToString() + " Coins";
        }

        for(int n=0; n< currentDay; n++)//for tick
        {
            RewardBoxes[n].transform.GetChild(3).gameObject.SetActive(true);
        }

        glowObj.transform.parent = RewardBoxes[currentDay].transform;
        glowObj.GetComponent<RectTransform>().anchoredPosition = RewardBoxes[currentDay].GetComponent<RectTransform>().anchoredPosition;
        glowObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0);
        glowObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        glowObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
    }

    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }
}
