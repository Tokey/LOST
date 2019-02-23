using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour {

    // Use this for initialization
    
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject splash;
    [SerializeField]
    private GameObject fader;
  
    float time;
    [SerializeField]
    float initialTime;
  
    

    void Start()
    {
        time = initialTime;
        PlayerPrefs.SetInt("CurrentLevel", 0);
    }
    void Update()
    {
        Debug.Log("HSlevel" + PlayerPrefs.GetInt("CurrentLevel", 0));
        time -= Time.deltaTime;

        if (time <= 2 && time>1)
        {
            fader.SetActive(true);
        }
        else if (time <= 1 && time >0)
        {
            splash.SetActive(false);
            main.SetActive(true);
        }
        else if (time <= 0)
        {
            fader.SetActive(false);
        }
    }

}
