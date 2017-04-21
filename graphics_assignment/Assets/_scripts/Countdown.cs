using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Countdown : MonoBehaviour
{
    public int testTime;

    private static float[] times = { 120f, 180f, 240f, 300f};

    private float timeLeft = 500;
    public bool stop = true;

    private float minutes;
    private float seconds;
    

    public void startTimer(float from)
    {
        timeLeft = times[testTime];
        stop = false;
        timeLeft = from;
        Update();
    }

    void Start()
    {
        timeLeft = times[testTime];
        stop = false;
        Update();
    }

    void Update()
    {
        if (stop) return;
        timeLeft -= Time.deltaTime;

        minutes = Mathf.Floor(timeLeft / 60);
        seconds = timeLeft % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            stop = true;
            minutes = 0;
            seconds = 0;
        }
        GetComponent<TextMesh>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //        fraction = (timeLeft * 100) % 100;
    }

    
}
