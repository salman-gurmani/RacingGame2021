using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionLevelBtnHandler : MonoBehaviour {
    
	[Header("GameObjects")]
	public GameObject lockObj;
	public Text buttonTxt;

	public int lvlNumber = 0;

	public GameObject [] star;

	public void SetButtonStats(int _lvlNum) {
		
		//Toolbox.GameManager.Log("N = " + _lvlNum);

		lvlNumber = _lvlNum;
		SetButtonTxt(_lvlNum + 1);

		if (Toolbox.DB.prefs.GameMode[Toolbox.DB.prefs.LastSelectedMode].LevelUnlocked[lvlNumber])
		{
			lockObj.SetActive(false);
		}
		else { 

			lockObj.SetActive(true);
		}

	}

    public void SetButtonTxt(int _val)
    {
		buttonTxt.text = "LEVEL "+ _val.ToString();
    }

    public void Press_LevelButton(){

		GetComponentInParent<LevelSelectionMenuListner>().OnPress_LevelButton(lvlNumber);
	}
	public void Press_LevelLockButton()
	{
		Toolbox.GameManager.InstantiatePopup_Message("This level is locked.");
	}
	public void DisableLock(){

		lockObj.SetActive (false);

	}

	public void EnableLock(){

		lockObj.SetActive (true);

	}
}
