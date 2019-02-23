using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject bMusic;
	void Start () {
        if (PlayerPrefs.GetInt("MusicToggle", 10) == 10)
            bMusic.SetActive(true);
        else
            bMusic.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
