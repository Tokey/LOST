using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextTest : MonoBehaviour {

    
    private int level;
    private Text txt;
    private string locktext;
    void Start()
    {
        PlayerPrefs.SetInt("LevelLock1", 10);
        txt = gameObject.GetComponent<Text>();
        string s = txt.text.ToString();
        level = int.Parse(s);
        locktext = "LevelLock" + level.ToString();
        if (!(PlayerPrefs.GetInt(locktext, 0) == 10))
        {
            this.gameObject.SetActive(false);
        }
        else
            this.gameObject.SetActive(true);

    }

 
}

