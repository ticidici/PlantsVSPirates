using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject scoreManager;
	public Camera m_Camera;
	private float timestamp_cooldown;
	public float cooldown = 1;
	
	void OnStart() {
		timestamp_cooldown = Time.time;
	}

	void LateUpdate () {
		if (Input.GetMouseButtonDown(0) && timestamp_cooldown <= Time.time)
         {
         	timestamp_cooldown = Time.time + cooldown;

            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "enemy" ) {
                	hit.transform.gameObject.SetActive(false);
                	scoreManager.SendMessage("addScore", 10);
                }
                else {
                	scoreManager.SendMessage("susbtractScore", 10);
                }
            }

         }
	}
}
