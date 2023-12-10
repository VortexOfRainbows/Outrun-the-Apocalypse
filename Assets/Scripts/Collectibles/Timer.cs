using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Timer : MonoBehaviour
{
    private const string LongestTime = "LongestTime";
    private const string FastestTime = "FastestTime";
    [SerializeField] private UIManager Manager;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text fastestTimeText;
    public static int Time { get; private set; }
    public static int RawSeconds => Time / 60;
    public int Minutes(int time)
    {
        int min = time / 3600;
        if (min > 99)
            min = 99;
        return min;
    }
    public int Seconds(int time)
    {
        return (time / 60) % 60;
    }
    private string AssembleTimeString(string concat, int time)
    {
        int min = Minutes(time);
        int sec = Seconds(time);
        string concat2 = ":";
        if (min < 10)
            concat += "0";
        if (sec < 10)
            concat += "0";
        return concat + min + concat2 + sec;
    }
    private void Start() 
    {
        Time = 0;
        timeText.text = AssembleTimeString("Time: ", Time);
        bestTimeText.text = AssembleTimeString("Longest Survival Time: ", PlayerPrefs.GetInt(LongestTime, 0));
        fastestTimeText.text = AssembleTimeString("Fastest Clear Time: ", PlayerPrefs.GetInt(FastestTime, int.MaxValue));
    }
    private void FixedUpdate()
    {
        Time++;
        timeText.text = AssembleTimeString("Time: ", Time);
        bool anythingChanged = false;
        int longestTime = PlayerPrefs.GetInt(LongestTime, 0);
        int fastestTime = PlayerPrefs.GetInt(FastestTime, int.MaxValue);
        if (Time > longestTime)
        {
            PlayerPrefs.SetInt(LongestTime, Time);
            bestTimeText.text = AssembleTimeString("Longest Survival Time: ", Time);
            anythingChanged = true;
        }
        if (Time < fastestTime && Manager.win) 
        {
            PlayerPrefs.SetInt(FastestTime, Time);
            fastestTimeText.text = AssembleTimeString("Fastest Clear Time: ", Time);
            anythingChanged = true;
            //Debug.Log(PlayerPrefs.GetInt("FastestHighScore"));
        }
        if(anythingChanged)
            PlayerPrefs.Save();
        Debug.Log(Time);
    }
}
