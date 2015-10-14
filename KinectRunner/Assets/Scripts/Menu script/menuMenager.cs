using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class menuMenager : MonoBehaviour
{
    public PlayerControl pc;
    public Texture2D gameOverTexture;
    public bool exit = false;
    public bool restartGame = false;
    

    public void sceneChange(string scaneName)
    {
        Application.LoadLevel(scaneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        if (pc.gameOver)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), gameOverTexture);
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 160, 30), "Score: " + pc.score, style);
            exit = GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 150, 100, 100), "Exit");
            restartGame = GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 50, 100, 100), "Restart");
            GUI.EndGroup();
        }
    }
}
