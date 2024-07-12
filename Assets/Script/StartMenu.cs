using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private void Start()
    {
        if (MazeGenerator.Instance == null)
        {
            //GameObject gameManager = new GameObject("MazeGen");
            //gameManager.AddComponent<MazeGenerator>();
        }
    }

    public void EasyMode()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMazeSize(new Vector2Int(25, 25));
            GameManager.Instance.SetPlayerStartPosition(new Vector3(-11.5f, 1, -11.5f)); // 원하는 위치로 설정
            SceneManager.LoadScene("MazeGameScene");
        }
    }

    public void MediumMode()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMazeSize(new Vector2Int(35, 35));
            GameManager.Instance.SetPlayerStartPosition(new Vector3(-16.5f, 1, -16.5f)); // 원하는 위치로 설정
            SceneManager.LoadScene("MazeGameScene");
        }
    }

    public void HardMode()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMazeSize(new Vector2Int(51, 51));
            GameManager.Instance.SetPlayerStartPosition(new Vector3(-24.5f, 1, -24.5f)); // 원하는 위치로 설정
            SceneManager.LoadScene("MazeGameScene");
        }
    }
}