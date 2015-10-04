using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

	public void sceneChange (string scaneName) 
    {
        Application.LoadLevel(scaneName);
	}

    public void exitGame()
    {
        Application.Quit();
    }
}
