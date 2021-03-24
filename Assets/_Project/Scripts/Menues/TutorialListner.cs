using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialListner : MonoBehaviour
{
    public Text titleTxt;
    public Text msgTxt;
    public GameObject[] character;

    [Header("Buttons")]
    public Button closeBtn;
    public Button nextBtn;

    public GameObject[] handObj;

    private void Start()
    {
        //if (Toolbox.GameplayScript)
        //    Toolbox.GameplayScript.DisableHud();

        closeBtn.onClick.AddListener(OnPress_Close);
        nextBtn.onClick.AddListener(OnPress_NextBtn);

        StartStateAction();
    }


    void NextButtonStatus(bool val) {

        nextBtn.gameObject.SetActive(val);
        //closeBtn.gameObject.SetActive(!val);
    }

    public void UpdateMsg(string str)
    {
        msgTxt.text = str;
    }

    public void UpdateMsg(string str, int charNum) {

        msgTxt.text = str;

        character[charNum].SetActive(true);
        
    }

    public void OnPress_Close() {

        if(Toolbox.GameplayScript)
            Toolbox.GameplayScript.EnableHud();

        Destroy(this.gameObject);
    }
    private void StartStateAction()
    {
        int curNum = 0;

        switch (curNum)
        {
            case 0:
                titleTxt.gameObject.SetActive(true);
                titleTxt.text = "Welcome to Dubai Expo 2020.";
                character[0].SetActive(true);
                //UpdateMsg("\n We're about to host an expo here in Dubai and we need your help to make it work. We were lucky to sign a contract with Siemens (Automation Company) and get resources to manage the mega-structure that Expo City is. Without any further ado,\n let's get started", 0);
               // Toolbox.GameplayScript.camListner.GetComponent<Lean.Touch.LeanDragTranslate_New>().enabled = false;
                //Toolbox.GameplayScript.DisableHud();
                NextButtonStatus(true);               

                break;

            case 1:

                UpdateMsg("First of all we need to Activate the mindsphere. Tap on the Mindsphere button to open the Mindsphere.", 0);
  
                handObj[0].SetActive(true);
                NextButtonStatus(false);

                break;

            case 2:

                UpdateMsg("Press the 'GO' button to build the Power Plant.", 0);
                handObj[1].SetActive(true);
                NextButtonStatus(false);

                break;

            case 3:

                UpdateMsg("Please wait for the construction to complete.", 1);
                NextButtonStatus(false);

                break;

            case 4:

                UpdateMsg("Great! Lets build the other building. Tap on the Mindsphere button to open the Mindsphere.", 0);
                handObj[0].SetActive(true);
                NextButtonStatus(false);

                break;

            case 5:

                UpdateMsg("Press the 'GO' button to build the Hydrogen Box.", 0);
                handObj[1].SetActive(true);
                NextButtonStatus(false);

                break;

            case 6:

                UpdateMsg("Please wait for the construction to complete.", 1);
                NextButtonStatus(false);

                break;

            case 7:

                UpdateMsg("Awesome! You have built all the buildings required for the Mindsphere. Lets Activate the Mindsphere now. Tap on the Mindsphere button to open the Mindsphere.", 0);
                handObj[0].SetActive(true);
                NextButtonStatus(false);

                break;

            case 8:

                UpdateMsg("Tap on next button to close this dialogue and tap on the 'Activate' button to activate Mindsphere. Best of luck!", 0);
                //handObj[1].SetActive(true);
                NextButtonStatus(true);

                break;

            case 9:

                UpdateMsg("Congratulations! Mindsphere has been activated. Now lets fix the first problem reported by Mindsphere. Tap on the Mindsphere button to open the Mindsphere.", 0);
                handObj[0].SetActive(true);
                NextButtonStatus(false);


                break;

            case 10:

                UpdateMsg("Tap on the 'Fix' button to fix the reported issue.", 0);
                handObj[1].SetActive(true);
                NextButtonStatus(true);

                break;

            case 11:

                UpdateMsg("Please wait for the issue to be fixed.", 1);
                //Toolbox.GameplayScript.DisableHud();
                NextButtonStatus(false);

                break;

            case 12:

                UpdateMsg("Great! Now you are ready to handle things on your own. Tap the next button to continue. Best of luck!", 0);
                NextButtonStatus(true);

                break;
        }

        StartTypewriterEffect();
    }

    public void OnPress_NextBtn()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        switch (0)
        {
            case 1:

                Toolbox.GameplayScript.EnableHud();

                break;

            case 13:
                
              //  Toolbox.GameplayScript.camListner.gameObject.GetComponent<Lean.Touch.LeanDragTranslate_New>().enabled = true;
               // Toolbox.GameplayScript.EnableHud();
               // Toolbox.HUDListner.Enable_AllButtons();   
                break;

        }

        Destroy(this.gameObject);
    }

    string story;

    void StartTypewriterEffect()
    {
        //txt = GetComponent<Text>();
        story = msgTxt.text;
        msgTxt.text = "";

        StartCoroutine(PlayText());
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            msgTxt.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
