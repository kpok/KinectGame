using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {

	//Material texture offset rate
	float speed = Utils.GROUND_SPEED;

	void Update () {
		float offset = Time.time * speed;                             
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -offset); 
	}
}
