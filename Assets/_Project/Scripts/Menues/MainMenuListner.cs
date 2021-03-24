using UnityEngine;
using UnityEngine.UI;

public class MainMenuListner : MonoBehaviour {

	public Text goldTxt;

    private void OnEnable()
    {
		UpdateTxt();
	}

    public void UpdateTxt(){

		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
	}

	public void OnPress_Shop()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Shop();
	}

	public void OnPress_Settings()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_SettingsMenu();	
	}

	public void OnPress_Fb()
	{
		Application.OpenURL(Constants.fb);
	}

	public void OnPress_RateUs()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Application.OpenURL(Constants.appLink);
	}
	public void OnPress_RemoveAds()
	{

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
	}

	public void OnPress_MoreGames()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Application.OpenURL(Constants.moreGamesLink);
	}

	public void OnPress_Back()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
		Toolbox.MenuHandler.Show_PrevUI();
	}


	public void OnPress_Next()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.MenuHandler.Show_NextUI();
	}

}
