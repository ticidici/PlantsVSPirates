using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    public const int MAX_ENEMIES = 10;
    private const float LEVEL_1_APPEAR_ENEMY_TIME = 5f;
    private const float LEVEL_2_APPEAR_ENEMY_TIME = 2f;
    private const float LEVEL_3_APPEAR_ENEMY_TIME = 1f;
    private const float LEVEL_4_APPEAR_ENEMY_TIME = 0.5f;

    public float appear_enemy_time = LEVEL_1_APPEAR_ENEMY_TIME;

    public GameObject scoreManager;
	public Camera m_Camera;
	public float cooldown = 1f;

	private float timestamp_cooldown;
	private float timer_activate_enemy = 0f;

    private List<FlowerController> flowers;
    private List<int> inactiveFlowers = new List<int>();
    private List<int> activeFlowers = new List<int>();
	
    void Awake() {
        //si hubiera más de un tipo de jardín, se tendría que cargar antes
        flowers = FindObjectsOfType<FlowerController>().ToList();
        Debug.Log("found flowers: " + flowers.Count);
        for(int i = 0; i < flowers.Count; i++) {
            flowers[i].SetId(i);
            inactiveFlowers.Add(i);
        }
        Debug.Log("inactive flowers: " + inactiveFlowers.Count);
        Debug.Log("active flowers: " + activeFlowers.Count);
    }
	
	void Start() {
		timestamp_cooldown = Time.time;
	}

    void LateUpdate () {
        //Debug 
        if (Input.GetKeyDown("space")) {
            GameObject go = GameObject.Find("Flower");
            go.SendMessage("StartGrowing");
        }
		if (Input.GetMouseButtonDown(0) && timestamp_cooldown <= Time.time) {
         	timestamp_cooldown = Time.time + cooldown;

            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "enemy" ) {
                    hit.transform.gameObject.SendMessage("deactivate");
                	scoreManager.SendMessage("addScore", 10);
                }
                else {
                	scoreManager.SendMessage("susbtractScore", 10);
                }
            }
        }
        if (timer_activate_enemy >= appear_enemy_time)
        {
            timer_activate_enemy = 0;
            activateEnemy();
        }
        else
        {
            timer_activate_enemy += Time.deltaTime;
        }
	}

    void activateEnemy()
    {
        if (activeFlowers.Count > 0)
        {

            int r_num = Random.Range(0, activeFlowers.Count);

            bool found = false;
            while (!found && r_num < activeFlowers.Count) {
                int flower_id = activeFlowers[r_num];

                if (!flowers[flower_id].hasEnemy()) {
                    flowers[flower_id].activateEnemy();
                    found = true;
                }
                else ++r_num;
            }
        }
	}

    public void ActivateFlower(int id) {
        bool found = false;
        foreach (int flowerID in inactiveFlowers) {
            if(flowerID == id) {
                found = true;
                break;
            }
        }
        if (found) {
            inactiveFlowers.Remove(id);
            activeFlowers.Add(id);
            if (activeFlowers.Count > 40) appear_enemy_time = LEVEL_4_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 20) appear_enemy_time = LEVEL_3_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 8) appear_enemy_time = LEVEL_2_APPEAR_ENEMY_TIME;
            else appear_enemy_time = LEVEL_1_APPEAR_ENEMY_TIME;
        }
        //TODO comprobar si ya están todas activas
    }

    public void DeactivateFlower(int id) {
        bool found = false;
        foreach(int flowerID in activeFlowers) {
            found = true;
            break;
        }
        if (found) {
            activeFlowers.Remove(id);
            inactiveFlowers.Add(id);
            if (activeFlowers.Count > 40) appear_enemy_time = LEVEL_4_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 20) appear_enemy_time = LEVEL_3_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 8) appear_enemy_time = LEVEL_2_APPEAR_ENEMY_TIME;
            else appear_enemy_time = LEVEL_1_APPEAR_ENEMY_TIME;
        }
    }

    public void makePlantAppear() {
        if(inactiveFlowers.Count > 0) {
            int newFlowerIndex = Random.Range(0, inactiveFlowers.Count);//el max es exclusivo
            flowers[inactiveFlowers[newFlowerIndex]].StartGrowing();
        }
    }
}
