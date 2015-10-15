using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

    public float objectSpeed = Utils.ITEM_SPEED;

	// Update is called once per frame
	void Update () {
		transform.Translate(0, objectSpeed, 0);
	}
}
