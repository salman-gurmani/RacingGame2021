using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNameListner : MonoBehaviour
{
    public InputField field;
    public Button btn;
    Color defaultColor;

    private void Start()
    {
        defaultColor = btn.GetComponent<Image>().color;
        btn.GetComponent<Image>().color = Color.grey;
        btn.enabled = false;
    }

    public void CheckReq() {

        if (field.text.Length > 2)
        {
            btn.GetComponent<Image>().color = defaultColor;
            btn.enabled = true;

        }
        else {
            btn.GetComponent<Image>().color = Color.grey;
            btn.enabled = false;
        }
    }
    public void Click_SetName() {
        
        Destroy(this.gameObject);
    }
}
