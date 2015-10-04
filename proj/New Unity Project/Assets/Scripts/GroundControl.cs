using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {


	//Material texture offset rate
	float speed = .5f;

	// Update is called once per frame
	void Update () {
		float offset = Time.time * speed;                             
		renderer.material.mainTextureOffset = new Vector2(0, -offset); 
	}
}
