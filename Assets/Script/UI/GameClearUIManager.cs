using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUIManager : MonoBehaviour
{
    public Text bestTimeText; // �ְ� ����� ǥ���� UI Text
    public Text currentTimeText; // ���� ����� ǥ���� UI Text

    private TimerManager timerManager;
    private float bestTime = Mathf.Infinity;

    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();

        // ���� �ְ� ����� �ε� (PlayerPrefs ���)
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

        // �ְ� ��� ���� ���� Ȯ��
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
        bestTimeText.text = string.Format("�ְ���: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void DisplayCurrentTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay * 1000) % 1000;
        currentTimeText.text = string.Format("�� ���: {0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
