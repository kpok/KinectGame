using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	
	public void sceneChange (string scaneName) {
        Application.LoadLevel(scaneName);
	}
}
