using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

    public const int MAX_HEALTH = 100;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int health;
    private int id = -1;


    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
        health = MAX_HEALTH;
    }

    void Update() {
        animator.SetInteger("health", health);
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
    }

    public void makeInvulnerable() {
        //avisar a game manager
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
        enemy.SetActive(true);
        enemy.SendMessage("activate");
    }

    public void deactivateEnemy()
    {
        GameObject enemy = GameObject.Find("Enemy" + id);
        enemy.SendMessage("deactivate");
    }
}
