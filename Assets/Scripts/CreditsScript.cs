using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour {

    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject credits;
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
    bool creditsChk;
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
            if (time < initialTime && time > initialTime / 2.0)
                fader.SetActive(true);
            else if (time <= initialTime / 2.0 && time > 0)
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
                credits.SetActive(false);
                main.SetActive(true);

            }
            else if (time <= 0)
            {
                fader.SetActive(false);
                time = initialTime;
                mainsChk = false;
            }


        }
        if (creditsChk)
        {
            time -= Time.deltaTime;

            if (time < initialTime && time > initialTime / 2.0)
                fader.SetActive(true);
            else if (time <= initialTime / 2.0 && time > 0)
            {
                settings.SetActive(false);
                credits.SetActive(true);

            }
            else if (time <= 0)
            {
                fader.SetActive(false);
                time = initialTime;
                creditsChk = false;
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

    public void CreditsToggle()
    {
        creditsChk = !creditsChk;
    }


}
