using GameAnalyticsSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[HideInInspector] public int nextSceneIndex = 0;

	[Header("Other")]
	[HideInInspector]
	public AsyncOperation async = null; // When assigned, load is in progress.

	[HideInInspector]
	public GameObject callingSureMenuGameobject;
	public float SceneDelay =1f;

    [Header("Debug")]
    public bool showDebug = false;

    public GameObject debugCanvas;
    public Text [] debugTxt;
    int debugCursor = 0;

	#region Notification

	private string noti_Id = "0";
	private int id;
    private AndroidNotificationChannel channel;
    private AndroidNotification notification;

    #endregion

    private void Start()
    {
		GameAnalytics.Initialize();

		AddNotificationChannel();

		if (Toolbox.DB.prefs.FirstRun)
		{
			Instantiate_Consent();
			Toolbox.DB.prefs.FirstTimeOpenTime = DateTime.Now;
			Toolbox.DB.prefs.FirstRun = false;

			Toolbox.DB.prefs.LastNotificationFireTime = DateTime.Now.AddHours(24);
			Schedule_Notification(Toolbox.DB.prefs.LastNotificationFireTime);
			Toolbox.DB.prefs.FuelTank = 8;

		}
		else {

			if(SceneManager.GetActiveScene().buildIndex == 0)
				LoadScene(Constants.sceneIndex_Menu, false, SceneDelay);
		}

		NotificationHandling();		
    }

    void LateUpdate(){


		if (async != null) {

			if (async.progress == 1) {

				async = null;
			}
		}
	}

	public void Set_DefaultValues(){


		Time.timeScale = 1;
	}

    public void DebugLogs_Status(bool val) {

        debugCanvas.SetActive(val);

    }

    public void Set_DebugLog(string str) {

        if (showDebug) {

            for (int i = 0; i < debugTxt.Length; i++)
            {
                debugTxt[i].color = Color.white;
            }

            debugTxt[debugCursor].text = str;
            debugTxt[debugCursor].color = Color.yellow;

            debugCursor++;

            if (debugCursor >= debugTxt.Length)
                debugCursor = 0;
        }
    }

	public void LoadScene(int _index, bool _showLoading, float _delay) {

		Toolbox.DB.Save_Binary_Prefs();
		nextSceneIndex = _index;

		StartCoroutine(CR_LoadScene(_index, _showLoading, _delay));
	}

	private IEnumerator CR_LoadScene(int _index, bool _showLoading, float _delay) {

		yield return new WaitForSeconds(_delay);

		GameObject loadingObj = new GameObject();

		if (_showLoading) {

			loadingObj = Instantiate_Loading();
		}		
		
		async = SceneManager.LoadSceneAsync(_index);
		yield return async;

		if(loadingObj && loadingObj.GetComponent<Loading>() && !loadingObj.GetComponent<Loading>().IsTemporaryLoading)
			Destroy(loadingObj);
	}

	private void NotificationHandling()
	{
		if (DateTime.Now >= Toolbox.DB.prefs.LastNotificationFireTime)
		{
			Toolbox.DB.prefs.LastNotificationFireTime = DateTime.Now.AddDays(1);			

			Schedule_Notification(Toolbox.DB.prefs.LastNotificationFireTime);
			Toolbox.GameManager.Log("NEW_Notification = " + Toolbox.DB.prefs.LastNotificationFireTime);
		}
	}

	public int GetLastSelectedLevelSceneIndex() {

		string path = Constants.PrefabFolderPath + Constants.LevelsScriptablesFolderPath + Toolbox.DB.prefs.LastSelectedLevel.ToString();
		LevelData curLevelData = (LevelData)Resources.Load(path);

		return curLevelData.sceneNum;
	}

	//Runtime Menu Handling
	public void Instantiate_PauseMenu(){
        
		Instantiate ((GameObject)Resources.Load (Constants.menuFolderPath + "Pause"));
	}
    
    public void Instantiate_LowCoins(){
        
		Instantiate ((GameObject)Resources.Load(Constants.menuFolderPath + "LowCoins"));
	}

	public void Instantiate_SureMenu(){

		Instantiate ((GameObject)Resources.Load(Constants.menuFolderPath + "SureMenu"));
	}
    
	public void Instantiate_OptionsMenu(){
        
		Instantiate ((GameObject)Resources.Load(Constants.menuFolderPath + "Options"));
	}

	public GameObject Instantiate_Loading(){

		return (GameObject) Instantiate ((GameObject)Resources.Load(Constants.menuFolderPath + "Loading"));
	}

	public void Instantiate_LevelComplete(float _delay)
	{
		GameObject obj = (GameObject)Resources.Load(Constants.menuFolderPath + "LevelComplete");

		StartCoroutine(CR_InstantiateObj( obj, _delay));
	}

	public void Instantiate_LevelFail(float _delay)
	{
		GameObject obj = (GameObject)Resources.Load(Constants.menuFolderPath + "LevelFail");

		StartCoroutine(CR_InstantiateObj(obj, _delay));
	}

	IEnumerator CR_InstantiateObj(GameObject _obj, float _delay)
	{
		yield return new WaitForSeconds(_delay);
		Instantiate(_obj);
	}

	private void Instantiate_TryAgainMenu(){

		Instantiate ((GameObject)Resources.Load(Constants.menuFolderPath + "TryAgain"));
	}

    public void Instantiate_SettingsMenu()
    {
        Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Settings"));
    }
	public void Instantiate_DailyRewardMenu()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Daily Reward"));
	}

	public void Instantiate_QuitMenu()
    {
        Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "QuitMenu"));
    }

	public void InstantiatePopup_Message(String str)
	{
		GameObject obj = Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Popup-Msg"));

		obj.GetComponent<PopupMsgListner>().UpdateMsg(str);
	}

	public void Instantiate_CoinsEffect()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Coins Effect"));
	}

	public void InstantiateRide_Msg(string _name, string _destination, string _description, float _timeLimit, float _speedLimit)
	{
		GameObject obj = Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Ride-Msg"));

		obj.GetComponent<PassengerMsgListner>().UpdateMsg(_name, _destination, _description, _timeLimit, _speedLimit);
	}

	public void InstantiatePopup_Tutorial()
    {
        if (FindObjectOfType<TutorialListner>())
            Destroy(FindObjectOfType<TutorialListner>().gameObject);

        Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Tutorial_Msg"));
    }

    public void InstantiatePopup_Tutorial(String str, int charac)
    {
        GameObject obj = Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Tutorial_Msg"));

        obj.GetComponent<TutorialListner>().UpdateMsg(str, charac);
    }

    public void Instantiate_Leaderboard()
    {
        Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Leaderboard"));
    }

	public void Instantiate_MainMenu()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "MainMenu"));
	}
	public void Instantiate_Tutorial()
	{
		Instantiate((GameObject)Resources.Load("Tutorial"));
	}
	public void Instantiate_SetNameMenu()
	{
		if(!FindObjectOfType<SetNameListner>())
			Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "SetName"));
	}

	public void Instantiate_Reward()
	{
		Instantiate((GameObject)Resources.Load("RewardEffect"));
	}

	public void Instantiate_CrossPromotionListner()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "CrossPromotion"));
	}
	public void Instantiate_Consent()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Consent"));
	}
	public void Instantiate_ReviewMenu()
	{
		if (Toolbox.DB.prefs.AppRated)
			return;

		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Review"));
	}
	public void Instantiate_DailyReward()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "DailyReward"));
	}
	public void Instantiate_Shop()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Store"));
	}	
	public void Instantiate_PaneltyMsg(String str)
	{
		GameObject obj = Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Panelty-Msg"));

		obj.GetComponent<PopupMsgListner>().UpdateMsg(str);

		Toolbox.HUDListner.IncrementPanelty(1);
	}

	public void Instantiate_Blackout()
	{
		Instantiate((GameObject)Resources.Load(Constants.menuFolderPath + "Blackout"));
	}

	#region Analytics

	public void Analytics_LevelStart() {

		Log("Analytics_Start");

		int _mode = Toolbox.DB.prefs.LastSelectedMode;
		int _level = Toolbox.DB.prefs.LastSelectedLevel;

		if(Toolbox.GameplayScript.islevelsScene)
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Mode_" + _mode, "_Level_" + _level);
		else
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Mode_" + _mode);
	}

	public void Analytics_LevelComplete()
	{
		Log("Analytics_Complete");

		int _mode = Toolbox.DB.prefs.LastSelectedMode;
		int _level = Toolbox.DB.prefs.LastSelectedLevel;

		if (Toolbox.GameplayScript.islevelsScene)
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Mode_" + _mode, "_Level_" + _level);
		else
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Mode_" + _mode);	
	}

	public void Analytics_LevelFail()
	{
		Log("Analytics_Fail");

		int _mode = Toolbox.DB.prefs.LastSelectedMode;
		int _level = Toolbox.DB.prefs.LastSelectedLevel;
				
		if (Toolbox.GameplayScript.islevelsScene)
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Mode_" + _mode, "_Level_" + _level);
		else
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Mode_" + _mode);
	
	}

	public void Analytics_Design(string _event)
	{
		if (!Toolbox.GameManager.IsNetworkAvailable())
			return;

		_event = _event.Replace(" ", "_");

		Log("AnalyticEvent_Design = (" + _event + ")");
		GameAnalytics.NewDesignEvent(_event);
		//AnalyticsEvent.Custom(_event);
	}

	public void AnalyticEvent_Business(string _transactionName, float _price, string _id) 
	{
		//GameAnalytics.NewBusinessEvent(_currency, _price, _type, _productName, _cartType);
		//AnalyticsEvent.IAPTransaction(_transactionName, _price, _id);
	}

	#endregion

	#region LOGS

	public void Log(string str)
	{
		//Debug.Log("<color=yellow> LOG -> </color>" + str);
	}

	public void Log(string str, string col)
	{
		//Debug.Log("<color=" + col + ">-> </color>" + str);
	}

	public void Log_ImplementationError(string str) {

		//Debug.Log("<color=red> LOG -> </color>" + str);
	}


	#endregion

	#region Local_Notification

	private void AddNotificationChannel() {

#if UNITY_ANDROID

		channel = new AndroidNotificationChannel()
		{
			Id = noti_Id,
			Name = "FirstReminder",
			Importance = Importance.High,
			Description = "This is the first reminder",
		};
		AndroidNotificationCenter.RegisterNotificationChannel(channel);
		Log("Channel Added = " + channel.Name);
#endif

	}

	public void Schedule_Notification(DateTime _time)
	{

#if UNITY_ANDROID

		string msg = "Lets, learn something new.";

		notification = new AndroidNotification();
		notification.Title = Application.productName;

		notification.Text = msg;		

		notification.FireTime = _time;
		notification.SmallIcon = "icon_1";
		notification.LargeIcon = "icon_0";

		id = AndroidNotificationCenter.SendNotification(notification, noti_Id);

#endif
	}

	public void CheckNotificationStatus() {
#if UNITY_ANDROID

		try
		{
			var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(id);

			if (notificationStatus == NotificationStatus.Scheduled)
			{
				// Replace the scheduled notification with a new notification.
				//AndroidNotificationCenter.UpdateScheduledNotification(id, newNotification, "channel_id");
				AndroidNotificationCenter.CancelNotification(id);
			}
			else if (notificationStatus == NotificationStatus.Delivered)
			{
				// Remove the previously shown notification from the status bar.
				AndroidNotificationCenter.CancelNotification(id);
			}
			else if (notificationStatus == NotificationStatus.Unknown)
			{
				//AndroidNotificationCenter.SendNotification(notification , channel);
				AndroidNotificationCenter.CancelNotification(id);
			}
		}
		catch (Exception ex) {

			Log(ex.ToString());
		}
		
#endif

	}

	#endregion

	public bool IsNetworkAvailable()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			//Toolbox.GameManager.InstantiatePopup_Message("Internet Not available. Please check your network and try again.");
			return false;
		}
		else
		{
			return true;
		}
	}

	void OnApplicationFocus(bool focus){

		if (!focus)
		{

			Debug.Log("Out of fucus");
		}
		else {

			//CheckNotificationStatus();
		}
	}

	void OnApplicationQuit(){

		Toolbox.DB.Save_Binary_Prefs();
    }
}
