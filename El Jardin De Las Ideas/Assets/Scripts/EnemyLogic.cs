using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

	public GameObject flower;
	private bool active = false;
	private float timer = 0f;
	public float timeAttack = 2f;
	
	// Update is called once per frame
	void LateUpdate () {
		if (active) {
			timer += Time.deltaTime;
			if (timer >= timeAttack) {
				timer = 0;
                float damage = Random.Range(15, 25);
				flower.SendMessage("LowerHealth", damage);
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
    }

    public void setFlower(string flower_name) {
        flower = GameObject.Find(flower_name);
    }
}
