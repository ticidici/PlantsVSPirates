using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

	public GameObject scoreManager;
	public Camera m_Camera;
	public float cooldown = 1;

	private float timestamp_cooldown;

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
		if (Input.GetMouseButtonDown(0) && timestamp_cooldown <= Time.time) {
         	timestamp_cooldown = Time.time + cooldown;

            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "enemy" ) {
                	hit.transform.gameObject.SendMessage("deactivate");
                	hit.transform.gameObject.SetActive(false);
                	scoreManager.SendMessage("addScore", 10);
                }
                else {
                	scoreManager.SendMessage("susbtractScore", 10);
                }
            }
        }
	}

    public void ActivateFlower(int id) {
        foreach (int flowerID in inactiveFlowers) {
            if(flowerID == id) {
                inactiveFlowers.Remove(flowerID);
                activeFlowers.Add(flowerID);
            }
        }
        //TODO comprobar si ya están todas activas
    }

    public void DeactivateFlower(int id) {
        foreach(int flowerID in activeFlowers) {
            activeFlowers.Remove(flowerID);
            inactiveFlowers.Add(flowerID);
        }
    }

    public void makePlantAppear() {
        if(inactiveFlowers.Count > 0) {
            int newFlowerIndex = Random.Range(0, inactiveFlowers.Count);//el max es exclusivo
            flowers[inactiveFlowers[newFlowerIndex]].StartGrowing();
        }
    }
}
