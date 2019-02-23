using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreViewer : MonoBehaviour {

    private Text _text;
    public int level;
    // Use this for initialization
    void Start()
    {
        level = PlayerPrefs.GetInt("CurrentLevel", 0);
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HSlevel" + level, 0);
        if (PlayerPrefs.GetInt("Slevel" + level, 0) > PlayerPrefs.GetInt("HSlevel" + level, 0))
            PlayerPrefs.SetInt("HSlevel" + level, PlayerPrefs.GetInt("Slevel" + level, 0));
        
    }
}
