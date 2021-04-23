using UnityEngine;
using UnityEngine.UI;

public class ShopListner : MonoBehaviour
{
    public Text goldTxt;
    public Text fuelTxt;

    private void OnEnable()
    {
        
    }
    private void Update()
    {
        UpdateTxt();
    }
    void UpdateTxt()
    {
        goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
        fuelTxt.text = Toolbox.DB.prefs.FuelTank.ToString();
    }

    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }

    public void OnPress_FreeCoins()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        AdsManager.instance.SetNShowRewardedAd(AdsManager.RewardType.FREECOINS, 100);
    }

    public void Purchase_1kCash() { InAppManager.instance.BuyCash_1000(); 
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes); }

    public void Purchase_3kCash() { InAppManager.instance.BuyCash_3000();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_5kCash() {InAppManager.instance.BuyCash_5000();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_20kCash() { InAppManager.instance.BuyCash_20000();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_50kCash() { InAppManager.instance.BuyCash_50000();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_UnlockLevels () {InAppManager.instance.Buy_UnlockAllLevels();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_UnlockCars() { InAppManager.instance.Buy_UnlockAllCars();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
    public void Purchase_Fuel() { InAppManager.instance.Buy_FuelTank();
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
    }
}
