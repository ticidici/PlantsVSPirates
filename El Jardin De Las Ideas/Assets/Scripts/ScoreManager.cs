using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public int score = 0;

	public void addScore (int value) {
		score += value;
	}

	public void susbtractScore (int value) {
		score -= value;
		if (score < 0 ) score = 0;
	}
	



}	
	