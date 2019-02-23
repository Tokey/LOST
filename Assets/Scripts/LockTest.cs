using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockTest : MonoBehaviour {

    [SerializeField]
    private int level;
    private  Text txt;
    private string locktext;
    void Start()
    {
        txt = this.gameObject.GetComponentInChildren<Text>();
        Debug.Log(txt);
        
        string s = txt.text.ToString();
        level = int.Parse(s);
        locktext = "LevelLock" + level.ToString();
        if (!(PlayerPrefs.GetInt(locktext, 0) == 10))
        {
            this.gameObject.GetComponent<Image>().enabled=true;
        }
        else
            this.gameObject.GetComponent<Image>().enabled = false;

    }
}
