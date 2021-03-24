using UnityEngine;
using System.Collections;

public class RotateAlways : MonoBehaviour {

	public float speed = 1;

	public bool xRotation = false;
	public bool yRotation = false;
	public bool zRotation = false;


	void FixedUpdate () {

		if (xRotation) {

			this.transform.Rotate(speed, 0 , 0 );
		}

		if (yRotation) {

			this.transform.Rotate (0, speed, 0);
		}

		if (zRotation) {

			this.transform.Rotate(0, 0 , speed );
		}

	}
}
