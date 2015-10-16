using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class scoreScript : MonoBehaviour
{

    private Score scores = Globals.score;

    public Text scoreText;

    void Start()
    {
        getScore();
    }

    public void getScore()
    {
        this.scoreText.text = "";
        foreach (playerScore ps in Globals.score.player)
        {
            this.scoreText.text += "Score: " + ps.nick +" - "+ps.score.ToString()+"\n";
        }
    }
}
