using UnityEngine;
using UnityEngine.UI;
using CnControls;

public class PlayerObjSelectionListner : MonoBehaviour
{
	public Text goldTxt;
	public Text fuelTxt;

	public int curIndex = 0;

	public GameObject environmentObj;
	public Transform objSpawnPoint;
	public Transform rotateObj;

	public GameObject spawnedObj;
	private PlayerObjData spawnedPlayerData;

	public GameObject playBtn;
	public GameObject unlockBtn;
	public float RotationSpeed;

	[Header("Specs")]
	public Text nameTxt;
	public Text priceCostTxt;
	public GameObject priceObj;
	public Image[] specs;

	private float xAxix;
	private void OnEnable()
	{
		UpdateTxt();
		curIndex = Toolbox.DB.prefs.LastSelectedPlayerObj;
		SpawnObject(curIndex);
	}


	public void UpdateTxt()
	{

		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
		fuelTxt.text = Toolbox.DB.prefs.FuelTank.ToString();
	}

    public void Update()
    {
        xAxix = CnControls.CnInputManager.GetAxis("Horizontal");

		rotateObj.transform.Rotate(Vector3.up, xAxix * Time.deltaTime * RotationSpeed);
    }
    private void SpawnObject(int _val)
	{

		string path = Constants.PrefabFolderPath + Constants.PlayerFolderPath + _val.ToString();
		//Toolbox.GameManager.Log("Vehicle path = " + path);

		if (spawnedObj)
			Destroy(spawnedObj);

		spawnedObj = (GameObject)Instantiate(Resources.Load(path), objSpawnPoint.position, objSpawnPoint.rotation, objSpawnPoint);

		path = Constants.PrefabFolderPath + Constants.PlayerScriptablesFolderPath + _val.ToString();

		spawnedPlayerData = (PlayerObjData)Resources.Load(path);

		UpdateUI();
	}

	public void OnPress_Prev()
	{
		if (curIndex - 1 < 0) {

			curIndex = Toolbox.DB.prefs.PlayerObjectBought.Length;
		}

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		curIndex--;
		SpawnObject(curIndex);
	}

	public void OnPress_Next()
	{
		if (curIndex + 1 >= Toolbox.DB.prefs.PlayerObjectBought.Length) {

			curIndex = -1;
		}

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		curIndex++;
		SpawnObject(curIndex);
	}
	public void OnPress_Shop()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Shop();
	}

	public void OnPress_Back()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);
		Toolbox.MenuHandler.Show_PrevUI();
	}

	public void OnPress_Play()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

		if (Toolbox.DB.prefs.PlayerObjectBought[curIndex])
		{
			Toolbox.DB.prefs.LastSelectedPlayerObj = curIndex;
			Toolbox.DB.prefs.LastSelectedVehicleName = spawnedPlayerData.name;
		}
		Toolbox.MenuHandler.Show_NextUI();
	}

	public void OnPress_carThumb(int no)
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		curIndex = no;
		SpawnObject(curIndex);
	}

	public void OnPress_Unlock()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

		if (Toolbox.DB.prefs.GoldCoins >= spawnedPlayerData.price)
		{

			Toolbox.DB.prefs.GoldCoins -= spawnedPlayerData.price;
			Toolbox.DB.prefs.PlayerObjectBought[curIndex] = true;

			Toolbox.GameManager.InstantiatePopup_Message("Congratulations! Vehicle Unlocked.");
		}
		else {
			Toolbox.GameManager.InstantiatePopup_Message("Sorry! You don't have enough money.");
		}

		

		UpdateUI();
		UpdateTxt();
	}
	private void UpdateUI()
	{

		if (Toolbox.DB.prefs.PlayerObjectBought[curIndex])
		{
			unlockBtn.SetActive(false);
			priceObj.gameObject.SetActive(false);
			playBtn.SetActive(true);
		}
		else
		{

			playBtn.SetActive(false);
			unlockBtn.SetActive(true);
			priceObj.gameObject.SetActive(true);
		}

		nameTxt.text = spawnedPlayerData.name;

		priceCostTxt.text = spawnedPlayerData.price.ToString();

		for (int i = 0; i < specs.Length; i++)
		{
			specs[i].fillAmount = spawnedPlayerData.specs[i];
		}

	}
}
