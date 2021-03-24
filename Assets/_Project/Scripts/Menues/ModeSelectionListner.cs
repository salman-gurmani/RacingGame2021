using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionListner : MonoBehaviour
{
	public Text goldTxt;
	public GameObject mode2Lock;

    private void Start()
    {
		HandleModeLock();
		UpdateCoins();

	}

	private void HandleModeLock() {

		if (Toolbox.DB.prefs.Mode2Unlocked)
			mode2Lock.SetActive(false);
	}

    void UpdateCoins()
	{
		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
	}

	public void OnPress_Back()
	{
			Toolbox.MenuHandler.Show_PrevUI();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);

	}

	public void OnPress_Mode(int _val)
	{
		Toolbox.DB.prefs.LastSelectedMode = _val;
		Toolbox.MenuHandler.Show_NextUI();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
	}

	public void OnPress_ModeLock(int _val) {

		Toolbox.GameManager.InstantiatePopup_Message("This mode is Locked. It will be available soon.");
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
	}
}
