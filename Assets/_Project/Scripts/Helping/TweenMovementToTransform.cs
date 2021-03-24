using UnityEngine;
public class TweenMovementToTransform : MonoBehaviour {

	[Header("Movement In Start")]
	public bool enableInStart = true;
	public float startDelay;


	public enum State{
		REACHPOINT
	}; 

	public State state;
    
	
	[SerializeField] private bool neverStop = true;
//	public bool takeLocalPos = true;
	public Transform target;

	Vector3 finalTargetPos;
	Vector3 startPosition;

	public float speed = 0.05f;
	public float stopingDistance = 5;

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

    [Header("On Reached")]
    public bool disableThisScriptOnReached = false;
  //  public LeanDragTranslate_New enableComponentOnReached;

    //	[Header("Custom Editor Options")]
    //	public bool setX = true;
    //	public bool setY = true;
    //	public bool setZ = true;


    void Start(){

		startPosition = this.transform.position;
        
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

	public void StartMovement(){

		enableMovement = true;
	} 

	void Update(){		

		if (!enableMovement && !target) {
		
			return;
		}
			
		switch (state) {

		case State.REACHPOINT:

			ReachPointMovement ();
			return;
		}
	}

	void ReachPointMovement(){	

		if (target) {

            this.transform.position = Vector3.Lerp(this.transform.position, target.position, speed);

            //if (this.transform.position == target.position && !neverStop) {

            //	enableMovement = false;


            //             if (enableComponentOnReached) {
            //                 enableComponentOnReached.enabled = false;
            //             }

            //             if (disableThisScriptOnReached) {

            //                 this.enabled = false;
            //             }

            //             Debug.LogError("Stoping Movement!");

            //         }

            //if (Vector3.Distance(this.transform.position, target.position) < stopingDistance && !neverStop)
            //{
            //    this.transform.position = target.position;
            //    enableMovement = false;


            //    //if (enableComponentOnReached)
            //    //{
            //    //    enableComponentOnReached.enabled = true;
            //    //}

            //    if (disableThisScriptOnReached)
            //    {

            //        this.enabled = false;
            //    }

            //    Debug.LogError("Stoping Movement!");

            //}
        }	
	}
}
