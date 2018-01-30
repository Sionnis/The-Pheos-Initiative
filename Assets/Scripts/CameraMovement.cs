using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public float speed = 15.0F;
	void Update() {
		float horizPan = Input.GetAxis ("Horizontal") * speed;
		float vertiPan = Input.GetAxis ("Vertical") * speed;
		horizPan *= Time.deltaTime;
		vertiPan *= Time.deltaTime;
		this.transform.Translate (horizPan, vertiPan, 0);
	}
}
