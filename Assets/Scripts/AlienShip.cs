using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShip : MonoBehaviour {

    private float y0;
    private float x0;

    public float amplitude;
    public float speed;

    public void Start()
    {
        y0 = transform.position.y;
        x0 = transform.position.x;
    }
    void Update () {
        Hover();
	}

    void Hover()
    {
        transform.position = new Vector3(x0 + amplitude * Mathf.Cos(speed * Time.time), y0 + amplitude * Mathf.Sin(speed * Time.time), 0f);

    }

  
}
