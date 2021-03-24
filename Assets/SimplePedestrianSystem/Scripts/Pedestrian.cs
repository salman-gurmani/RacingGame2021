using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PedestrianSystem{

	public class Pedestrian : MonoBehaviour {

		public enum MovementType
		{
			IDLE,
			WALK, // will walk
			RUN // will run
		}
		[Tooltip("Movement type of the pedestrian")]
		public MovementType movementType;

		private MovementType defaultMovementType;

		[Tooltip("Target to which player will move and face towards")]
		public Transform target;

		[Tooltip("Pedestrian will walk with this speed")]
		[Header("Values")]
		public float walkSpeed = 0.2f;
		[Tooltip("Pedestrian will run with this speed")]
		public float runSpeed = 0.2f;
		[Tooltip("Pedestrian will rotate with this speed")]
		public float rotationSpeed = 1;

		bool isDestroyed = false;

		public SkinnedMeshRenderer meshRend;
		public Animator anim;
		public GameObject ragdoll;

		private Transform spawner;
		private float maxDistRadius;

		public Transform raycastPoint;
		public float sensorLength = 5;
		Vector3 fwd;

		public AudioClip [] deadSound;
        public Transform Spawner { get => spawner; set {

				spawner = value; 
				maxDistRadius = spawner.GetComponent<SphereCollider>().radius + 5;
			} }

		private float distTimeCheck = 5;

        private void OnEnable()
        {
			isDestroyed = false;
		}

		//set animator value of pedestrian according to the state choosen
		void Start(){

			defaultMovementType = movementType;
			distTimeCheck += Random.Range(5, 10);

			UpdateAnimator();

		}

		void UpdateAnimator() {

			switch (movementType)
			{

				case MovementType.IDLE:

					anim.SetInteger("MoveState", 0);

					return;

				case MovementType.WALK:

					anim.SetInteger("MoveState", 1);

					return;

				case MovementType.RUN:

					anim.SetInteger("MoveState", 2);

					return;
			}
		}

		void Update(){

			HandleAnimatorStatus();
			PedestrianMovement ();
			DistanceCheckFromSpawnerHandling();

			SensorCheck();

			// if target is assinged. Rotate toward it accordingly
			if (target) {
			
				Quaternion targetRotation = Quaternion.LookRotation (target.position - this.transform.position, this.transform.up);
				targetRotation.x = 0; targetRotation.z = 0;
				this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
			}
		}

        private void SensorCheck()
        {
			fwd = raycastPoint.TransformDirection(Vector3.forward);

			if (Physics.Raycast(raycastPoint.position, fwd, sensorLength))
			{

				//Debug.Log("-> Object Infront");

				if (movementType != MovementType.IDLE)
				{
					movementType = MovementType.IDLE;
					UpdateAnimator();
				}
			}
			else {

				if (movementType != defaultMovementType)
				{
					movementType = defaultMovementType;
					UpdateAnimator();
				}
			}
			
		}

        private void DistanceCheckFromSpawnerHandling() {

			distTimeCheck -= Time.deltaTime;

			if (distTimeCheck <= 0) {

				if (Vector3.Distance(this.transform.position, spawner.position) > maxDistRadius) {

					DestroyPedestrian(spawner.GetComponent<PedestrianSpawner>().pediSystemManager);
				}

				distTimeCheck += Random.Range(5, 10);
			}
		}

		private void HandleAnimatorStatus() {

			if (meshRend.isVisible)
			{
				anim.enabled = true;
			}
			else {
				anim.enabled = false;
			}
		}

		//movement acfording to movement type
		void PedestrianMovement(){

			switch (movementType) {

			case MovementType.IDLE:

					//this.transform.position += this.transform.forward * Time.deltaTime * walkSpeed;

					return;

			case MovementType.WALK:

				this.transform.position += this.transform.forward * Time.deltaTime * walkSpeed;

				return;

			case MovementType.RUN:

				this.transform.position += this.transform.forward * Time.deltaTime * runSpeed;

				return;
			}
		}


		public void Dead()
		{
			movementType = MovementType.IDLE;

			Instantiate(ragdoll, this.transform.position, this.transform.rotation);

			//anim.SetTrigger("Dead");
			//StartCoroutine(DisableAfterTime(2));

			Toolbox.Soundmanager.PlaySound(deadSound[Random.Range(0, deadSound.Length)]);

			movementType = MovementType.WALK;
			DestroyPedestrian(spawner.GetComponent<PedestrianSpawner>().pediSystemManager);
		}

		IEnumerator DisableAfterTime(int _time) {

			yield return new WaitForSeconds(_time);

			movementType = MovementType.WALK;
			DestroyPedestrian(spawner.GetComponent<PedestrianSpawner>().pediSystemManager);
		}

		//properly destroy current pedestrian
		public void DestroyPedestrian(PedestrianSystemManager pedestrianSystem){

			if (!isDestroyed)
			{
				isDestroyed = true;

				if (pedestrianSystem.usePooling)
				{
					pedestrianSystem.curPedestiansSpawned--;
					this.gameObject.SetActive(false);
				}
				else
				{
					isDestroyed = true;

					pedestrianSystem.curPedestiansSpawned--;
					Destroy(this.gameObject);
					
				}
			}
		
		}

		//move to next waypoint once hit the target waypoint
		public void OnTriggerEnter(Collider col){

			if(col.CompareTag("Waypoint")){

				if(col.gameObject == target.gameObject)
					target = col.GetComponent<Waypoint> ().GetNextWaypoint ();
			}

			//if (col.CompareTag("Player"))
			//{
			//	Debug.LogError("Player!");
			//}
		}

	}
}
