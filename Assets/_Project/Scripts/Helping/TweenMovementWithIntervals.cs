using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMovementWithIntervals : MonoBehaviour {

	[Header("Movement In Start")]
	public bool enableInStart = false;


	public enum State{
		REACHININTERVALS,
		INFINITELOOP
	}; 

	[Header("Other")]
	public State state;

	[Tooltip("Delay before reaching next position. 0 is start point.")]
	public float [] intervals;
	int curInterval = 0;
	public int infiniteInterval = 0;

	float time = 0;

	bool isBack = false;
	bool runTime = false;

	//	public bool takeLocalPos = true;
	public Vector3 targetPos;

	Vector3 finalTargetPos;
	Vector3 startPosition;

	public float speed = 0.05f;

	bool enableMovement = false;
	bool targetSwich = false;

	[Header("Random Generate Value")]
	public float minRandDelay = 0;
	public float maxRandDelay = 1;

	[Header("Fixed Generate Value")]
	public float maxFixedDelay = 1;

	void Start(){

		startPosition = this.transform.position;

		targetPos = startPosition + targetPos;

		finalTargetPos = targetPos;

		if (enableInStart) {

			curInterval = 0;

			if (state == State.REACHININTERVALS) {
			
				time = intervals [curInterval];
			} else {
			
				time = infiniteInterval;
			}


			runTime = true;
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

		TimeHandling ();

		if (!enableMovement) {

			return;
		}

		switch (state) {

		case State.REACHININTERVALS:

			LoopInInterval ();

			return;

		case State.INFINITELOOP:

			InfiniteLoop ();

			return;
		}
	}

	void TimeHandling (){

		if (runTime) {

			time -= Time.deltaTime;

			if (time <= 0) {

				if (state == State.REACHININTERVALS) {

					time = intervals [curInterval];
				} else {

					time = infiniteInterval;
				}

				runTime = false;

				enableMovement = true;
			}
		}
	}

	void LoopInInterval(){

		this.transform.position = Vector3.Slerp (this.transform.position, targetPos, speed);

		if (Vector3.Distance(this.transform.position, targetPos) < 0.1f ) {

			if (!isBack) {

				ToggleTarget ();

				isBack = true;

			}else {

				enableMovement = false;

				curInterval++;

				if (curInterval < intervals.Length) {

					time = intervals [curInterval];
				
				} else {
				
					this.enabled = false;
				}

				isBack = false;

				runTime = true;
			}
		} 
	}

	void InfiniteLoop(){
	
		this.transform.position = Vector3.Slerp (this.transform.position, targetPos, speed);

		if (Vector3.Distance(this.transform.position, targetPos) < 0.1f ) {

			if (!isBack) {

				ToggleTarget ();

				isBack = true;

			}else {

				enableMovement = false;

				time = infiniteInterval;

				isBack = false;

				runTime = true;
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

}
