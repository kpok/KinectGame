using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    private List<int> scoreList = new List<int>(10);

    public Text scoreText;

    void Start()
    {
        getScore();
    }

    public void getScore()
    {
        this.scoreText.text = "";
        foreach (int score in scoreList)
        {
            this.scoreText.text += "Score: " + score.ToString()+"\n";
        }
    }
}
