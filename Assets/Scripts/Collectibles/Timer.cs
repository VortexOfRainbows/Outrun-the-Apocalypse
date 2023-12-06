using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //public static Timer instance;
    public TMP_Text timeText;
    public TMP_Text bestTimeText;
    
    private float time;
    private float minutes;
    private float seconds;

    void Awake()
    {
        //instance = this;
    }

    private void Start() 
    {
        timeText.text = "Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        bestTimeText.text = "Longest Survival Time:  " + PlayerPrefs.GetFloat("HighScoreMin", 0).ToString() + ":" + PlayerPrefs.GetFloat("HighScoreSec", 0).ToString("00");
    }
    private void Update()
    {
        //if (instance != this || instance == null)
        //    instance = this;

        time += Time.deltaTime;
        seconds += Time.deltaTime;

        if (seconds > 60) 
        {
            seconds = 0;
            minutes += 1;
        }

        timeText.text = "Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        //Debug.Log(PlayerPrefs.GetFloat("Highscore") + ", " + time);
        //Debug.Log(PlayerPrefs.GetFloat("HighscoreSec") + ", " + seconds);

        if (time > PlayerPrefs.GetFloat("HighScore", 0))
        { 
            PlayerPrefs.SetFloat("HighScore", time);
            PlayerPrefs.SetFloat("HighScoreMin", minutes);
            PlayerPrefs.SetFloat("HighScoreSec", seconds);

            PlayerPrefs.Save();

            //Debug.Log(PlayerPrefs.GetFloat("HighScore"));

            bestTimeText.text = "Longest Survival Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        }
    }
}
