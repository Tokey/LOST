using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VibrationButton : MonoBehaviour {

    private Image mButton;
    void Start()
    {
        mButton = gameObject.GetComponent<Image>();
    }

    void Update()
    {

    }

    public void Toggle()
    {

        if (mButton.color == new Color(0.3f, 0.3f, 0.3f))
            mButton.color = Color.white;
        else
            mButton.color = new Color(0.3f, 0.3f, 0.3f);
    }
}
