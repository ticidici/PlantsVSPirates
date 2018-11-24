using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public int score = 0;
	// Use this for initialization
	public void addScore (int value) {
		Debug.Log("addScore");
		score += value;
	}

	public void susbtractScore (int value) {
		Debug.Log("substractScore");
		score -= value;
		if (score < 0 ) score = 0;
	}
	



}	
	