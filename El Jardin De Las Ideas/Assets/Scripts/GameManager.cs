using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

	public GameObject scoreManager;
	public Camera m_Camera;

    private List<FlowerController> flowers;
    //dos vectores más con las activas e inactivas, con su id, que es la posición como han quedado en la lista
	
    void Awake() {
        //si hubiera más de un tipo de jardín, se tendría que cargar antes
        flowers = FindObjectsOfType<FlowerController>().ToList();
        Debug.Log("found flowers: " + flowers.Count);
    }

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
