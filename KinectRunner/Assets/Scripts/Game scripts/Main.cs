using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {


	/*private static Main instance = null;
	public static Main Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (Main)FindObjectOfType(typeof(Main));
			}
			return instance;
		}
	}*/

	void Awake()
	{
		Globals.LoadConfig();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Application.Quit");
			Application.Quit();
		}
		Globals.GameActiveTime += Time.deltaTime;
	}
}
