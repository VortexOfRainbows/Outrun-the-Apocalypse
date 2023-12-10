using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //public static Timer instance;

    public UIManager Manager;

    public TMP_Text timeText;
    public TMP_Text bestTimeText;
    public TMP_Text fastestTimeText;
    
    private float time;
    private float minutes;
    private float seconds;

    void Awake()
    {
        //instance = this;
    }

    private void Start() 
    {
        time = 0;
        PlayerPrefs.GetFloat("FastestHighScore", 9999f);
        timeText.text = "Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        bestTimeText.text = "Longest Survival Time:  " + PlayerPrefs.GetFloat("HighScoreMin", 0).ToString() + ":" + PlayerPrefs.GetFloat("HighScoreSec", 0).ToString("00");
        fastestTimeText.text = "Fastest Clear Time:  " + PlayerPrefs.GetFloat("FastestHighScoreMin").ToString() + ":" + PlayerPrefs.GetFloat("FastestHighScoreSec").ToString("00");
    }
    private void Update()
    {
        //if (instance != this || instance == null)
        //    instance = this;

        time = time + (Time.deltaTime * Time.timeScale);

            seconds = time % 60;
            minutes =  Mathf.FloorToInt(time / 60);

        timeText.text = "Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");

        if (time > PlayerPrefs.GetFloat("HighScore", 0))
        { 
            PlayerPrefs.SetFloat("HighScore", time);
            PlayerPrefs.SetFloat("HighScoreMin", minutes);
            PlayerPrefs.SetFloat("HighScoreSec", seconds);

            PlayerPrefs.Save();

            //Debug.Log(PlayerPrefs.GetFloat("HighScore"));

            bestTimeText.text = "Longest Survival Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        }

        if (time < PlayerPrefs.GetFloat("FastestHighScore", 9999f) && Manager.win) 
        {
            PlayerPrefs.SetFloat("FastestHighScore", time);
            PlayerPrefs.SetFloat("FastestHighScoreMin", minutes);
            PlayerPrefs.SetFloat("FastestHighScoreSec", seconds);

            Debug.Log(PlayerPrefs.GetFloat("FastestHighScore"));
            fastestTimeText.text = "Fastest Clear Time:  " + minutes.ToString() + ":" + Mathf.Round(seconds).ToString("00");
        }

        //Debug.Log(time < PlayerPrefs.GetFloat("FastestHighScore", 9999f));
        //Debug.Log(PlayerPrefs.GetFloat("FastestHighScore", 9999f));
        Debug.Log(time);

    }
}
