using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelector : MonoBehaviour {

    // Use this for initialization
    
    
    [SerializeField]
    private GameObject[] planetSelectos;
    [SerializeField]
    private GameObject fader;
    
    float time;
    [SerializeField]
    float initialTime;
    bool next;
    bool prev;
    int current;
    // Update is called once per frame

    private void Start()
    {
        time = initialTime;
    }
    void Update () {
        if (next)
        {
            fader.SetActive(true);
            time -= Time.deltaTime;
            if (time <= initialTime/2.0)
            {
                planetSelectos[current].SetActive(false);
                planetSelectos[current+1].SetActive(true);
            }
            if (time <= 0)
            {
                fader.SetActive(false);
                next = false;
                time = initialTime;
            }
        }
        if (prev)
        {
            time -= Time.deltaTime;
            fader.SetActive(true);
            if (time <= initialTime / 2.0)
            {
                if (current == 1)
                {
                    for (int i = 1; i < planetSelectos.Length; i++)
                        planetSelectos[i].SetActive(false);
                    planetSelectos[0].SetActive(true);
                    
                }
                else
                {
                    planetSelectos[current].SetActive(false);
                    planetSelectos[current - 1].SetActive(true);
                }
            }
            if (time <= 0)
            {
                fader.SetActive(false);
                prev = false;
                time = initialTime;
            }
        }

    }

    public void GoNext(int cur)
    {
        current = cur;
        next = true;
    }
    public void GoPrev(int cur)
    {
        current = cur;
        prev = true;
    }
}
