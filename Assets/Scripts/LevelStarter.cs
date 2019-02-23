using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour {

    // Use this for initialization
    public int level;
	void Start () {
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.SetInt("Slevel" + level, 0);
        PlayerPrefs.SetInt("LevelLock" + level, 10);

    }
	
	
}
