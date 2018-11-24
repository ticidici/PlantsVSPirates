using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    public const int MAX_HEALTH = 100;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private int health;
    private int id = -1;

    public const float TIME_PUNTUATE = 3f;
    private float timer_puntuation = 0f; 

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        health = MAX_HEALTH;
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

    public void activateEnemy()
    {
        GameObject enemy = GameObject.Find("Enemy" + id);
        if (enemy != null) {
            enemy.SetActive(true);
            enemy.SendMessage("activate");
        }
    }

    public void deactivateEnemy()
    {
        GameObject enemy = GameObject.Find("Enemy" + id);
        if (enemy != null)
            enemy.SendMessage("deactivate");
    }
}
