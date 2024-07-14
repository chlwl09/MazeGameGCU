using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUIManager : MonoBehaviour
{
    public Text bestTimeText; // 최고 기록을 표시할 UI Text
    public Text currentTimeText; // 현재 기록을 표시할 UI Text

    private TimerManager timerManager;
    private float bestTime = Mathf.Infinity;

    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();

        // 이전 최고 기록을 로드 (PlayerPrefs 사용)
        if (PlayerPrefs.HasKey("BestTime"))
        {
            bestTime = PlayerPrefs.GetFloat("BestTime");
            DisplayBestTime(bestTime);
        }
    }

    public void OnGameClear()
    {
        timerManager.StopTimer();
        float currentTime = timerManager.currentTime;
        DisplayCurrentTime(currentTime);

        // 최고 기록 갱신 여부 확인
        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            DisplayBestTime(bestTime);
        }
    }

    void DisplayBestTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay * 1000) % 1000;
        bestTimeText.text = string.Format("최고기록: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void DisplayCurrentTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay * 1000) % 1000;
        currentTimeText.text = string.Format("내 기록: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
