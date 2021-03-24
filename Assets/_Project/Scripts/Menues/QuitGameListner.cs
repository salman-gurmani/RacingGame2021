using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameListner : MonoBehaviour {

	//public Text message;


	void Update(){
	
		if (Input.GetKeyDown (KeyCode.Escape)) {

			OnPress_No ();
		}
	}

	public void OnPress_Yes(){
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Application.Quit ();
	}

	public void OnPress_No(){
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);

        Destroy(this.gameObject);
	}

}
