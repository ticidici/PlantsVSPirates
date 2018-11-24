using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

	public GameObject flower;
	private bool active = false;
	private float timer = 0;
	public float timeAttack = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (active) {
			timer += Time.deltaTime;
			if (timer >= timeAttack) {
				timer = 0;
				//flower.SendMessage("dropLife");
			}
		}
	}

    public void deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public void activate()
    {
        active = true;
        Debug.Log("JONAS");
    }

}
