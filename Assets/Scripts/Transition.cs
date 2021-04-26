using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image Screen;
    bool lastStatus = true;

    void Start()
    {
        Screen = GameObject.Find("TransitionImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastStatus == GameManager.Instance.Probe.AISleep)
            return;
        else
            lastStatus = GameManager.Instance.Probe.AISleep;

        if (GameManager.Instance.Probe.AISleep)
            StartCoroutine(FadeOut());
        else
            StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float increment = 0.1f;
       
        for (float x = increment; x <= 1; x += increment)
        {
            Screen.color = new Color(0, 0, 0, x);
            yield return new WaitForSeconds(increment);
        }
        Screen.color = new Color(0, 0, 0, 1);
    }

    private IEnumerator FadeIn()
    {
        float increment = -0.1f;

        for (float x = 1; x >= 0; x += increment)
        {
            Screen.color = new Color(0, 0, 0, x);
            yield return new WaitForSeconds(increment);
        }

        Screen.color = new Color(0, 0, 0, 0);
    }
}
