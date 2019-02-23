using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public float time;
    public  BarScript timerBar;
    public GameObject levelFailed;
    int fuse = 0;

    void Start () {
        timerBar.MaxValue = time;
        timerBar.Value = time;
    }
	
	// Update is called once per frame
	void  Update () {
        if (time > 0f)
        {
            time -= Time.deltaTime;
            timerBar.Value = time;
        }
        else if (time <= 0f)
        {
            if (fuse == 0)
            {
                GameManager.Instance.Pause2();
                levelFailed.SetActive(true);
                fuse = 1;
            }

        }
    }
  
}
