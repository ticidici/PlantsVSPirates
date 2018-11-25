using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    public const int MAX_HEALTH = 100;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private GameObject my_enemy;

    private int health;
    private int id = -1;
    private bool enemy_activated = false;

    [FMODUnity.EventRef]
    public string GrowEvent;
    [FMODUnity.EventRef]
    public string DeathEvent;

    FMOD.Studio.EventInstance Sound;

    public const float TIME_PUNTUATE = 3f;
    private float timer_puntuation = 0f; 

    void Start() {
        my_enemy = gameObject.transform.GetChild(0).gameObject;
        my_enemy.SendMessage("setFlower", gameObject.name);
        my_enemy.SendMessage("deactivate");
    }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        health = MAX_HEALTH;
        //StartGrowing();
    }

    void Update() {
        animator.SetInteger("health", health);

        if (health > 0) {
            if (timer_puntuation >= TIME_PUNTUATE) {
                timer_puntuation = 0f;
                GameObject score_manager = GameObject.Find("ScoreManager");
                animator = GetComponent<Animator>();
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle_healthy"))
                    score_manager.SendMessage("addScore", 100);
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle_hurt"))
                    score_manager.SendMessage("addScore", 80);
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle_almost_dead"))
                    score_manager.SendMessage("addScore", 50);
            }
            else {
                timer_puntuation += Time.deltaTime;
            }
        }
    }

    public void LowerHealth(int damage) {
        health -= damage;
        if (health < 1) {
            makeInvulnerable();
        }
    }

    public void setMaxHealth() {
        health = MAX_HEALTH;
    }

    public void makeVulnerable() {
        //avisar a game manager
        gameManager.ActivateFlower(id);
    }

    public void makeInvulnerable() {
        //avisar a game manager
        gameManager.DeactivateFlower(id);
        deactivateEnemy();
    }

    public void StartGrowing() {
        animator.SetTrigger("start_growing");
    }

    public void ResetFlower() {
        animator.SetTrigger("reset");
    }

    public void SetId(int assignedId) {
        id = assignedId;
        Debug.Log(gameObject.name +": My Id is " + id);
    }

    public void ThrowSoundEvent(string eventName) {
        if(eventName == "grow") {
            eventName = GrowEvent;
        }else if (eventName == "death"){
            eventName = DeathEvent;
        }
        Sound = FMODUnity.RuntimeManager.CreateInstance(eventName);
        //Sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Sound.start();
    }

    public void activateEnemy()
    {
        if (my_enemy != null) {
            enemy_activated = true;
            my_enemy.SetActive(true);
            my_enemy.SendMessage("activate");
        }
    }

    public void deactivateEnemy()
    {
        if (my_enemy != null)
            my_enemy.SendMessage("deactivate");
    }

    public bool hasEnemy() {
        return enemy_activated;
    }
}
