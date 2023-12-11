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
    private const string DefaultText = "Current: ";
    private const string LongestText = "Longest: ";
    private const string FastestText = "Completion: ";
    [SerializeField] private UIManager Manager;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text fastestTimeText;
    public static int CurrentTime => (int)(AccumulatedTime * 60);
    public static float AccumulatedTime { get; private set; }
    public static int RawSeconds => CurrentTime / 60;
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
        if(time >= int.MaxValue)
        {
            return concat + "N/A";
        }
        int min = Minutes(time);
        int sec = Seconds(time);
        string concat2 = ":";
        if (min < 10)
            concat += "0";
        if (sec < 10)
            concat2 += "0";
        return concat + min + concat2 + sec;
    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll(); <-- player prefs should be reset for testing purposes
        AccumulatedTime = 0;
        timeText.text = AssembleTimeString(DefaultText, CurrentTime);
        bestTimeText.text = AssembleTimeString(LongestText, PlayerPrefs.GetInt(LongestTime, 0));
        fastestTimeText.text = AssembleTimeString(FastestText, PlayerPrefs.GetInt(FastestTime, int.MaxValue));
        Manager.RecordWinTime = false;
    }
    private void Update()
    {
        AccumulatedTime += Time.deltaTime;
        timeText.text = AssembleTimeString(DefaultText, CurrentTime);
        bool anythingChanged = false;
        int longestTime = PlayerPrefs.GetInt(LongestTime, 0);
        int fastestTime = PlayerPrefs.GetInt(FastestTime, int.MaxValue);
        if (CurrentTime > longestTime)
        {
            PlayerPrefs.SetInt(LongestTime, CurrentTime);
            bestTimeText.text = AssembleTimeString(LongestText, CurrentTime);
            anythingChanged = true;
        }
        if (CurrentTime < fastestTime && Manager.RecordWinTime) 
        {
            PlayerPrefs.SetInt(FastestTime, CurrentTime);
            fastestTimeText.text = AssembleTimeString(FastestText, CurrentTime);
            anythingChanged = true;
            //Debug.Log(PlayerPrefs.GetInt("FastestHighScore"));
        }
        if(anythingChanged)
            PlayerPrefs.Save();
        //Debug.Log(CurrentTime);
    }
}
