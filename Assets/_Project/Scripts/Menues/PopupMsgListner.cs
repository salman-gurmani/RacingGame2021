using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsgListner : MonoBehaviour
{
    public Text msgTxt;
    public GameObject unlockBtn;
    public void UpdateMsg(string str) {

        msgTxt.text = str;
    }

    public void OnPress_Close() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Destroy(this.gameObject);
    }

    public void OnPress_UnlockMega() {

        OnPress_Close();
    }

    public void OnPress_PrivacyPolicy()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Application.OpenURL(Constants.privacyPolicy);
    }

    public void OnPress_AgreeOfConsent()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Toolbox.DB.prefs.FirstRun = false;
        Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, false, 0);
    }
}
