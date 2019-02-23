using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    // Use this for initialization
    public Text coinText;
    public Text health;
    public Text jpFuel;
    public Text jpForce;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        coinText.text ="COINS: "+ PlayerPrefs.GetInt("Points", 0).ToString();
        health.text = "HEALTH: "+ PlayerPrefs.GetInt("Health", 100).ToString()+"\n $"+ (PlayerPrefs.GetInt("hpCoins", 20000) * PlayerPrefs.GetInt("hpMul", 1)).ToString();
        jpFuel.text = "JETPACK FUEL: " + PlayerPrefs.GetInt("Fuel", 5).ToString() + "\n $" + (PlayerPrefs.GetInt("jfCoins", 30000) * PlayerPrefs.GetInt("jfMul", 1)).ToString();
        jpForce.text = "JETPACK FORCE: " + PlayerPrefs.GetInt("Force", 3).ToString() + "\n $" + (PlayerPrefs.GetInt("jfrCoins", 50000) * PlayerPrefs.GetInt("jfrMul", 1)).ToString();


    }

    public void Health()
    {
        if (PlayerPrefs.GetInt("Points", 0) >= (PlayerPrefs.GetInt("hpCoins", 20000) * PlayerPrefs.GetInt("hpMul", 1)))
        {
            PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health", 100) + 10);
            PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) - (PlayerPrefs.GetInt("hpCoins", 20000) * PlayerPrefs.GetInt("hpMul", 1)));
            PlayerPrefs.SetInt("hpMul", PlayerPrefs.GetInt("hpMul", 1) + 1);
        }
    }
    public void Fuel()
    {
        if (PlayerPrefs.GetInt("Points", 0) >= (PlayerPrefs.GetInt("jfCoins", 20000) * PlayerPrefs.GetInt("jfMul", 1)))
        {
            PlayerPrefs.SetInt("Fuel", PlayerPrefs.GetInt("Fuel", 5) + 1);
            PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) - (PlayerPrefs.GetInt("jfCoins", 30000) * PlayerPrefs.GetInt("jfMul", 1)));
            PlayerPrefs.SetInt("jfMul", PlayerPrefs.GetInt("jfMul", 1) + 1);
        }
    }
    public void Force()
    {
        if (PlayerPrefs.GetInt("Points", 0) >= (PlayerPrefs.GetInt("jrfCoins", 20000) * PlayerPrefs.GetInt("jrfMul", 1)))
        {
            PlayerPrefs.SetInt("Force", PlayerPrefs.GetInt("Force", 3) + 1);
            PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("Points", 0) - (PlayerPrefs.GetInt("jrfCoins", 50000) * PlayerPrefs.GetInt("jrfMul", 1)));
            PlayerPrefs.SetInt("jfrMul", PlayerPrefs.GetInt("jfrMul", 1) + 1);
        }
    }

}
