using UnityEngine;

namespace PedestrianSystem
{

    public class Waypoint : MonoBehaviour {

		[Tooltip("Pedestrian System Manager Reference")]
		public PedestrianSystemManager manager;

		[Tooltip("Next waypoint Reference. Target will move to this point once hit this waypoint")]
		public Waypoint nextWaypoint;

		bool canSpawnPediOnThisWaypoint = true;
		float time = 2;

        private void Update()
        {
			if (time > 0) {

				time -= Time.deltaTime;

				if (time <= 0) {

					canSpawnPediOnThisWaypoint = true;
					time = 0;
				}
			}
        }

        //Instantiate a pedestrian at this waypoint
        public void Instatiate_Pedestrian(){

			//Debug.Log("InitPedi!");

			if (!canSpawnPediOnThisWaypoint)
				return;

			if (manager.CanSpawnPedi ()) {

				GameObject pedi = null;

				if (manager.usePooling)
				{
					//Debug.Log("in Pool!");

					pedi = manager.Get_AvailablePoolPedestrian();
					//Debug.Log("Name = " + pedi.name);
					pedi.SetActive(true);
					pedi.transform.position = this.transform.position;
					pedi.transform.rotation = this.transform.rotation;
				}
				else {
					//instantiate pedestrian
					pedi = (GameObject)Instantiate(manager.Get_Pedestrian(), this.transform.position, this.transform.rotation);
				}				

				//assign target to pedestrian. It will moove toward its target
				pedi.GetComponent<Pedestrian> ().target = GetNextWaypoint ();

				//increment spawned pedestrians
				manager.curPedestiansSpawned++;

				canSpawnPediOnThisWaypoint = false;
				time = 2;

			} else {
			
				Debug.Log ("FULL");
			}
		}

		//return the transform of next waypoint
		public Transform GetNextWaypoint(){
		
			if (nextWaypoint)
				return this.nextWaypoint.transform;
			else
				return null;
		}
	}
}