using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoGota : MonoBehaviour {

    public float velocity = 0f;

	// Update is called once per frame
	void Update () {
        transform.position -= Vector3.up * velocity * Time.deltaTime;
    }
}
