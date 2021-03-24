using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDir : MonoBehaviour {

	public enum State{
		pos_X,
		neg_X,
		pos_Y,
		neg_Y,
		pos_Z,
		neg_Z

	}; public State state;

	public float speed = 1;
	public bool global = false;

	void Update () {
	
		switch (state) {

		case State.pos_X:

			if (global)
				this.transform.position += Vector3.right * Time.deltaTime * speed;
			else
				this.transform.position += this.transform.right * Time.deltaTime * speed;
				
			return;

		case State.neg_X:

			if (global)
				this.transform.position += (-Vector3.right) * Time.deltaTime * speed;
			else
				this.transform.position += (-this.transform.right) * Time.deltaTime * speed;

			return;
		
		case State.pos_Y:

			if (global)
				this.transform.position += (Vector3.up) * Time.deltaTime * speed;
			else
				this.transform.position += (this.transform.up) * Time.deltaTime * speed;

			return;

		case State.neg_Y:

			if (global)
				this.transform.position += (-Vector3.up) * Time.deltaTime * speed;
			else
				this.transform.position += (-this.transform.up) * Time.deltaTime * speed;

			return;

		case State.pos_Z:

			if (global)
				this.transform.position += (Vector3.forward) * Time.deltaTime * speed;
			else
				this.transform.position += (this.transform.forward) * Time.deltaTime * speed;

			return;

		case State.neg_Z:

			if (global)
				this.transform.position += (-Vector3.forward) * Time.deltaTime * speed;

			else
				this.transform.position += (-this.transform.forward) * Time.deltaTime * speed;

			return;
		}

	}
}
