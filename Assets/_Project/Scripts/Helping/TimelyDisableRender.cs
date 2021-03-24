using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelyDisableRender : MonoBehaviour {

	float time = 0;
	public float disableTime = 1;

	void OnEnable () {

		time = 0;
	}

	void Update(){
	
		time += Time.deltaTime;

		if (time >= disableTime) {
		
			this.gameObject.SetActive (false);
		}
	}
}
