using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndsOnDeath : MonoBehaviour {

    public LevelController timeLeft;
    public Text hostageText;
    public GameObject instance;
    int level;
    public Health playerHP;
    bool fuse = false;
    // Use this for initialization
    void Start()
    {
        hostageText.text = "Defeat The Boss: " + playerHP.CurrentHealth.ToString();
    }
    private ParticleSystem _psystem;
    void Awake()
    {
        _psystem = GetComponent<ParticleSystem>();
        level = PlayerPrefs.GetInt("CurrentLevel", 0);
    }


    private void Update()
    {
        hostageText.text = "Defeat The Boss: " + playerHP.CurrentHealth;
        if (playerHP.CurrentHealth <= 0 && !fuse)
        {
            fuse = true;
            PlayerPrefs.SetInt("Slevel" + level, PlayerPrefs.GetInt("Slevel" + level, 0) + 10000);
            ToNextLevel();
        }
    }

    void ToNextLevel()
    {
        PlayerPrefs.SetInt("Slevel" + level, PlayerPrefs.GetInt("Slevel" + level, 0) + (int)timeLeft.time * 50);
        GameManager.Instance.Pause2();
        instance.SetActive(true);
        
    }
}
