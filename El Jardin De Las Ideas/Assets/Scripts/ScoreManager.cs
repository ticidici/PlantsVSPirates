using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int score = 0;
    public Text scoreText;

    public void addScore (int value) {
		score += value;
        scoreText.text = "Score: " + score;

    }

    public void susbtractScore (int value) {
		score -= value;
		if (score < 0 ) score = 0;
        scoreText.text = "Score: " + score;

    }

    public int getScore()
    {
        return score;
    }






}	
	