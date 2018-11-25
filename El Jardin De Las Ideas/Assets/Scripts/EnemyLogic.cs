using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

	public GameObject flower;
	private bool active = false;
	private float timer = 0f;
	public float timeAttack = 2f;

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    public int life = 0;
	
    void Start() {
        life1 = gameObject.transform.GetChild(0).gameObject;
        life2 = gameObject.transform.GetChild(1).gameObject;
        life3 = gameObject.transform.GetChild(2).gameObject;

        life1.SetActive(false);
        life2.SetActive(false);
        life3.SetActive(false);
    }
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
        flower.SendMessage("deactiveEnemy");

        active = false;
        life1 = gameObject.transform.GetChild(0).gameObject;
        life2 = gameObject.transform.GetChild(1).gameObject;
        life3 = gameObject.transform.GetChild(2).gameObject;

        life1.SetActive(false);
        life2.SetActive(false);
        life3.SetActive(false);
        gameObject.SetActive(false);
    }

    public void activate()
    {
        active = true;
        life = Random.Range(1, 4);
        life1.SetActive(true);
        if (life > 1)
            life2.SetActive(true);
        if (life > 2)
            life3.SetActive(true);

    }

    public void setFlower(string flower_name) {
        flower = GameObject.Find(flower_name);
    }

    public void hit() {
        --life;
        if (life < 1) {
            deactivate();
        }
        else if (life == 1) {
            life2.SetActive(false);
            life3.SetActive(false);
        }
        else if (life == 2) {
            life3.SetActive(false);
        }
    }
}
