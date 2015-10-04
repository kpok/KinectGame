using UnityEngine;
using System.Collections;

public class PowerupScript: MonoBehaviour {

	public float objectSpeed = -0.5f;
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, 0, objectSpeed);
	}
}
