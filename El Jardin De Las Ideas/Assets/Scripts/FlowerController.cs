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

    [FMODUnity.EventRef]
    public string GrowEvent;
    [FMODUnity.EventRef]
    public string DeathEvent;

    FMOD.Studio.EventInstance Sound;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        health = MAX_HEALTH;
        StartGrowing();
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

    public void ThrowSoundEvent(string eventName) {
        if(eventName == "grow") {
            eventName = GrowEvent;
        }else if (eventName == "death"){
            eventName = DeathEvent;
        }
        Sound = FMODUnity.RuntimeManager.CreateInstance(eventName);
        Sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Sound.start();
    }
}
