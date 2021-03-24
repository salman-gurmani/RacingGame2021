using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenMovement : MonoBehaviour {

	[Header("Movement In Start")]
	public bool enableInStart = true;
	public float startDelay;

	public enum State{
		REACHPOINT,
		LOOP

	}; 

	[Header("Other")]
	public State state;

	RectTransform recTransform;
	public Vector3 targetPos;

	Vector3 finalTargetPos;
	Vector3 startPosition;

	public float speed = 0.05f;

	[Header("Loop State Specific")]
	int curcycle = 0;
	public int cycle = 0;

	bool enableMovement = false;
	bool targetSwich = false;

	[Header("Random Generate Value")]
	public float minRandDelay = 0;
	public float maxRandDelay = 1;

	[Header("Fixed Generate Value")]
	public float maxFixedDelay = 1;

	void Start(){

		startPosition = this.transform.position;
		recTransform = this.GetComponent<RectTransform> ();

		targetPos = startPosition + targetPos;

		finalTargetPos = targetPos;

		if (enableInStart) {
		
			Invoke ("StartMovement", startDelay);
		}
	}

	public void StartMovementwithRandromDelay(){

		float rand = Random.Range (minRandDelay, maxRandDelay);

		Invoke ("StartMovement", rand);
	}

	public void StartMovementwithFixedDelay(){

		Invoke ("StartMovement", maxFixedDelay);
	}

	void StartMovement(){

		enableMovement = true;
	}

	void Update(){
		

		if (!enableMovement) {
		
			return;
		}

		switch (state) {

		case State.REACHPOINT:

			ReachPointMovement ();

			return;

		case State.LOOP:

			ComeBackMovement ();

			return;
		}

	}

	void ReachPointMovement(){

		transform.position = Vector2.Lerp (this.transform.position, targetPos, speed);

		if (this.transform.position == targetPos) {

			enableMovement = false;
		}
	
	}

	void ComeBackMovement(){

		//Debug.LogError("A");

		this.transform.position = Vector2.Lerp (this.transform.position, targetPos, speed);

		if (Vector3.Distance(this.transform.position, targetPos) < 0.1f ) {
			//Debug.LogError("B");

			//if (curcycle < cycle) {
			
				ToggleTarget ();

				//curcycle++;
			
			//}
			//else {

			//	enableMovement = false;
			//}

		} 

	}

	void ToggleTarget(){

		//Debug.LogError("Toggling target");

		if (targetSwich) {
		
			targetPos = startPosition;

			targetSwich = false;

		} else {
		
			targetPos = finalTargetPos;

			targetSwich = true;		
		}

	}


}
