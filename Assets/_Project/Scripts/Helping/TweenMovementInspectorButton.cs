using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMovementInspectorButton : MonoBehaviour {

	[Header("Movement In Start")]
	public bool enableInStart = false;
	public float startDelay;


	public enum State{
		REACHPOINT,
		LOOP

	}; 

	[Header("Other")]
	public State state;

	//	public bool takeLocalPos = true;
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

		[Header("Custom Editor Options")]
		public bool setX = true;
		public bool setY = true;
		public bool setZ = true;


	void Start(){

		startPosition = this.transform.position;

		SetTargetPos ();

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

		this.transform.position = Vector3.Slerp (this.transform.position, targetPos, speed);

		if (this.transform.position == targetPos) {

			enableMovement = false;
		}

	}

	void ComeBackMovement(){

		this.transform.position = Vector3.Slerp (this.transform.position, targetPos, speed);

		if (Vector3.Distance(this.transform.position, targetPos) < 0.1f ) {

			if (curcycle < cycle) {

				ToggleTarget ();

				curcycle++;

			}else {

				enableMovement = false;
			}

		} 

	}

	void ToggleTarget(){

		if (targetSwich) {

			targetPos = startPosition;

			targetSwich = false;

		} else {

			targetPos = finalTargetPos;

			targetSwich = true;		
		}

	}

	void SetTargetPos(){
	
		if (!setX) {
		
			targetPos = new Vector3 (transform.position.x, targetPos.y, targetPos.z);
		}

		if (!setY) {

			targetPos = new Vector3 (targetPos.x, transform.position.y, targetPos.z);
		}

		if (!setZ) {

			targetPos = new Vector3 (targetPos.x, targetPos.y, transform.position.z);
		}
	}


	//Called From Editor
	public void SetCurPosAsTargetPosition(){

		if (setX && setY && setZ) {
			targetPos = this.transform.position;
		
		} else {
		
			if (setX) {

				targetPos = new Vector3 (transform.position.x, targetPos.y, targetPos.z);
			}

			if (setY) {

				targetPos = new Vector3 (targetPos.x, transform.position.y, targetPos.z);
			}

			if (setZ) {

				targetPos = new Vector3 (targetPos.x, targetPos.y, transform.position.z);
			}
		
		}

	}


}
