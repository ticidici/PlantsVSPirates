using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject scoreManager;
	public Camera m_Camera;
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown(0))
         {
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
