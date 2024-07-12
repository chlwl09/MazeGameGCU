using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float startTime;
    private bool isPlaying;
    public Text timeText;  // UI Text ������Ʈ�� �巡�� �� ������� �Ҵ�
    public float currentTime;

    void Start()
    {
        startTime = Time.time;
        isPlaying = true;
    }

    void Update()
    {
        if (isPlaying)
        {
            currentTime = Time.time - startTime;
            DisplayTime(currentTime);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay * 1000) % 1000;
        timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isPlaying = false;
    }

    public void StartTimer()
    {
        isPlaying = true;
        startTime = Time.time;
    }
}
