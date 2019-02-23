using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i <= 20; i++)
        {
            PlayerPrefs.SetInt("LevelLock"+i.ToString(), 10);
        }

        PlayerPrefs.SetInt("Points", 800000);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
