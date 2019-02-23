using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour {

    private Image mButton;
    [SerializeField]
    private GameObject bMusic;
	void Start () {
        mButton = gameObject.GetComponent<Image>();
        if (PlayerPrefs.GetInt("MusicToggle", 10) != 10)
        {
            bMusic.SetActive(false);
            mButton.color = new Color(0.3f, 0.3f, 0.3f);
        }
        else
        {
            bMusic.SetActive(true);
            mButton.color = Color.white;
        }
    }
	
	void Update () {
       
    }

    public void Toggle()
    {

        if (mButton.color == new Color(0.3f, 0.3f, 0.3f))
        {
            mButton.color = Color.white;
            PlayerPrefs.SetInt("MusicToggle", 10);
        }
        else
        { mButton.color = new Color(0.3f, 0.3f, 0.3f);
            PlayerPrefs.SetInt("MusicToggle", 0);
        }

        if (PlayerPrefs.GetInt("MusicToggle", 10) != 10)
            bMusic.SetActive(false);
        else
            bMusic.SetActive(true);
    }
}
