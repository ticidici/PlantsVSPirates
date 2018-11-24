using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoGota : MonoBehaviour {

    private float velocity = 0f;

    void Awake()
    {
        velocity = Random.Range(Constants.MIN_VELOCITY_DROP, Constants.MAX_VELOCITY_DROP); ;
    }

	// Update is called once per frame
	void Update () {
        transform.position -= Vector3.up * velocity * Time.deltaTime;
    }
}
