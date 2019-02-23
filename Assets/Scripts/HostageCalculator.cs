using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostageCalculator : MonoBehaviour {

    public LevelController timeLeft;
    public Text hostageText;
    public int totalHostages;
    public GameObject instance;
     int level;
	// Use this for initialization
	void Start () {
        hostageText.text = "Lost Aliens Left: " + totalHostages.ToString();
    }
    private ParticleSystem _psystem;
    void Awake()
    {
        _psystem = GetComponent<ParticleSystem>();
        level = PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Hostage")
        {
            Destroy(collider.gameObject);
            _psystem.Play();
            totalHostages--;
            hostageText.text = "Lost Aliens Left: " + totalHostages.ToString();
            PlayerPrefs.SetInt("Slevel" + level, PlayerPrefs.GetInt("Slevel" + level, 0)+100);
            if (totalHostages == 0)
                ToNextLevel();
        }

    }

    void ToNextLevel()
    {
        PlayerPrefs.SetInt("Slevel" + level, PlayerPrefs.GetInt("Slevel" + level, 0) + (int)timeLeft.time * 50);
        instance.SetActive(true);
        GameManager.Instance.Pause2();
    }
}
