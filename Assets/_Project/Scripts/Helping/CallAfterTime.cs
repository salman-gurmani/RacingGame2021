using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CallAfterTime : MonoBehaviour {



	float t ;
	bool methodCalled = false;
	public GameObject objToInstantiate;
//--------------------------------------------

	public float DelayTime = 0; // this is the time after which some method will be called

	public enum State{
		CALLSCENE,
        DESTROY,
        INSTANTIATE_OBJ_NonStop,
		OTHER

	}; public State state;

	public string sceneName = null ; 
	public string storySceneName = null ; 


	// Use this for initialization
	void Start () {
		
		t = Time.time;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Time.time == t + DelayTime && !methodCalled) {

		
			Work ();
			methodCalled = true;
		}

	}

	void Work (){

		switch (state) {

		case State.CALLSCENE:
		{
			CallScene ();
			break;
		}

        case State.DESTROY:
        {
            Destroy(this.gameObject);
            break;
        }

        case State.INSTANTIATE_OBJ_NonStop:
		{

			InstantiateObjRecursively ();
			break;
		}
				//add other cases here incase of more delay states to be performed
			
		default:
			Debug.LogError ("None of the cases ran in CallAfterTime Script");
			break;

		}
	}

	void CallScene(){
		
		if (sceneName != null) {

			SceneManager.LoadScene (sceneName);

		} else {				
			Debug.LogError ("Please Enter the Scene Name to be loaded In SCENENAME string");
		}
		
	}

	void InstantiateObjRecursively(){

		Instantiate (objToInstantiate, this.transform.position, Quaternion.identity);

		t = Time.time;;
		methodCalled = false;


	}

}
