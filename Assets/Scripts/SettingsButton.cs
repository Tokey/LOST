using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject fader;
    [SerializeField]
    float time;
    [SerializeField]
    float initialTime;

    bool settingsChk;
    bool mainsChk;
    void Start()
    {
        time = initialTime;
        settingsChk = false;
        mainsChk = false;
    }
    void Update()
    {
        if (settingsChk)
        {
            time -= Time.deltaTime;
            if(time<initialTime && time > initialTime / 2.0)
            fader.SetActive(true);
            else if (time <= initialTime / 2.0 && time>0)
            {
                
                    settings.SetActive(true);
                    main.SetActive(false);
                
                
            }
            else if (time <= 0)
            {
                fader.SetActive(false);
                settingsChk = false;
                time = initialTime;
            }
           
                
            
        }
        if (mainsChk)
        {
            time -= Time.deltaTime;

            if (time < initialTime && time > initialTime / 2.0)
                fader.SetActive(true);
            else if (time <= initialTime / 2.0 && time > 0)
            {
                settings.SetActive(false);
                main.SetActive(true);

            }
            else if (time <= 0)
            {
                fader.SetActive(false);
                time = initialTime;
                mainsChk = false;
            }
            
                
            
        }
    }

    public void SettingsToggle()
    {
        settingsChk = !settingsChk;
    }
    public void MainsToggle()
    {
        mainsChk = !mainsChk;
    }
}
