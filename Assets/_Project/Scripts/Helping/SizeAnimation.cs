using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeAnimation : MonoBehaviour {

	public float startDelay = 0;
	
	public float minSize = 1;
	public float maxSize = 1.5f;

	public float speed = 0.005f;

	bool mStart = false;
	
	bool isMaxSize = false;

	public enum state
	{
		HEARBEAT,
		POPUP
	}

	public state animationState = state.HEARBEAT;

	void OnEnable(){
	
		Invoke ("Initialize", startDelay);

		if (animationState == state.POPUP)
			this.transform.localScale = Vector3.zero;
	}

	void Initialize(){
	
		mStart = true;
	}

	void Update(){

		if (!mStart)
			return;

		switch (animationState) {

			case state.HEARBEAT:

				if (isMaxSize)
				{

					GoLow();
				}
				else
				{

					GoHigh();
				}

				break;


			case state.POPUP:

				PopUp();

				break;
		}
	}

	public void PopUp()
	{

		this.transform.localScale = new Vector3(this.transform.localScale.x + speed,
													this.transform.localScale.y + speed,
														this.transform.localScale.z + speed);

		if (this.transform.localScale.x > maxSize)
		{
			mStart = false;
		}


	}
	public void GoLow(){

		this.transform.localScale = new Vector3 (this.transform.localScale.x + speed, 
													this.transform.localScale.y + speed, 
														this.transform.localScale.z + speed);
		
		if (this.transform.localScale.x > maxSize) {
						
			isMaxSize = false;		
		}

	}

	public void GoHigh(){
	

		this.transform.localScale = new Vector3 (this.transform.localScale.x - speed, 
													this.transform.localScale.y - speed, 
														this.transform.localScale.z - speed);

		if (this.transform.localScale.x <= minSize) {

			isMaxSize = true;
		}
	}
}
