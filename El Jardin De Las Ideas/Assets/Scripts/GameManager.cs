using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	public float cooldown = 0.1f;

	private float timestamp_cooldown;
	private float timer_activate_enemy = 0f;

    private List<FlowerController> flowers;
    private List<int> inactiveFlowers;
    private List<int> activeFlowers;

    public float timeLeft = 180f;
    public Text timeLeftText;
    public Text gameOverText;
    public GameObject gameover_background;
    private bool game_over = false;

    [FMODUnity.EventRef]
    public string FailedShotEvent;
    [FMODUnity.EventRef]
    public string impactShotEvent;


    FMOD.Studio.EventInstance Sound;

    void Awake() {
        //si hubiera más de un tipo de jardín, se tendría que cargar antes
        flowers = FindObjectsOfType<FlowerController>().ToList();
        Debug.Log("found flowers: " + flowers.Count);
        inactiveFlowers = new List<int>();
        activeFlowers = new List<int>();
        for (int i = 0; i < flowers.Count; i++) {
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
        if (!game_over)
        {
            if (Input.GetMouseButtonDown(0) && timestamp_cooldown <= Time.time)
            {
                timestamp_cooldown = Time.time + cooldown;

                RaycastHit hit;
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "enemy")
                    {
                        hit.transform.gameObject.SendMessage("hit");
                        scoreManager.SendMessage("addScore", 30);
                        Sound = FMODUnity.RuntimeManager.CreateInstance(impactShotEvent);
                        Sound.start();
                    }
                    else
                    {
                        scoreManager.SendMessage("susbtractScore", 150);
                        Sound = FMODUnity.RuntimeManager.CreateInstance(FailedShotEvent);
                        Sound.start();
                    }
                }
            }
            if (timer_activate_enemy >= appear_enemy_time)
            {
                timer_activate_enemy = 0f;
                activateEnemy();
            }
            else
            {
                timer_activate_enemy += Time.deltaTime;
            }
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0f;
            gameOver();

        }
        timeLeftText.text = "Time left: " + timeLeft.ToString("n2") + "s";

    }

    void activateEnemy()
    {
        if (activeFlowers.Count > 0 && !game_over)
        {
            int r_num = Random.Range(0, activeFlowers.Count);
            int first_num = r_num;

            bool found = false;
            while (!found && r_num < activeFlowers.Count) {
                int flower_id = activeFlowers[r_num];

                if (!flowers[flower_id].hasEnemy()) {
                    flowers[flower_id].activateEnemy();
                    found = true;
                }
                else ++r_num;

                if (r_num >= activeFlowers.Count) r_num = 0;

                if (r_num == first_num) found = true;
            }
        }
	}

    public void ActivateFlower(int id) {
        bool found = false;
        foreach (int flowerID in inactiveFlowers) {
            if(flowerID == id) {
                found = true;
                //break;
            }
        }
        if (found) {
            inactiveFlowers.Remove(id);
            activeFlowers.Add(id);
            if (activeFlowers.Count > 14) appear_enemy_time = LEVEL_4_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 10) appear_enemy_time = LEVEL_3_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 5) appear_enemy_time = LEVEL_2_APPEAR_ENEMY_TIME;
            else appear_enemy_time = LEVEL_1_APPEAR_ENEMY_TIME;
        }
        //TODO comprobar si ya están todas activas
        if(inactiveFlowers.Count < 1) {
            Debug.Log("Todas las plantas activas!");
        }
    }

    public void DeactivateFlower(int id) {
        bool found = false;
        foreach(int flowerID in activeFlowers) {
            if (flowerID == id)
            {
                found = true;
            }
            //break;
        }
        if (found) {
            activeFlowers.Remove(id);
            inactiveFlowers.Add(id);
            if (activeFlowers.Count > 14) appear_enemy_time = LEVEL_4_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 10) appear_enemy_time = LEVEL_3_APPEAR_ENEMY_TIME;
            else if (activeFlowers.Count > 5) appear_enemy_time = LEVEL_2_APPEAR_ENEMY_TIME;
            else appear_enemy_time = LEVEL_1_APPEAR_ENEMY_TIME;
        }
    }

    public void makePlantAppear() {
        //Debug.Log("inactive flowers: " + inactiveFlowers.Count);
        //Debug.Log("active flowers: " + activeFlowers.Count);
        if (inactiveFlowers.Count > 0) {

            int newFlowerIndex = Random.Range(0, inactiveFlowers.Count);//el max es exclusivo

            flowers[inactiveFlowers[newFlowerIndex]].StartGrowing();
            //lo quitamos del inactive pero no lo ponemos en active aún, para que no se pueda intentar volver a poner
            //inactiveFlowers.RemoveAt(newFlowerIndex);
        }
        else {
            Debug.LogWarning("Se debería haber reseteado el jardín ya");
        }
    }

    public void gameOver()
    {
        game_over = true;
        gameover_background.SetActive(true);
        gameOverText.text = "Game Over\n Score: " + scoreManager.GetComponent<ScoreManager>().getScore();
    }

    public bool getGameOver()
    {
        return game_over;
    }
}
