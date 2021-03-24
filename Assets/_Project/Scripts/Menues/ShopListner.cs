using UnityEngine;
using UnityEngine.UI;

public class ShopListner : MonoBehaviour
{
    public Text goldTxt;

    private void OnEnable()
    {
        UpdateTxt();
    }

    void UpdateTxt()
    {
        goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
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

    public void PurchaseProduct(int _val)
    {

        switch (_val) {


            case 0:

                Toolbox.InAppHandler.BuyProductID(Constants.coins_1);

                break;

            case 1:

                Toolbox.InAppHandler.BuyProductID(Constants.coins_2);
                break;

            case 2:

                Toolbox.InAppHandler.BuyProductID(Constants.unlockPlayerObj);
                break;

            case 3:

                Toolbox.InAppHandler.BuyProductID(Constants.unlockLevels);
                break;

        }

    }
}
