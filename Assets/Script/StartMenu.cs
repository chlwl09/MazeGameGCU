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
            GameObject gameManager = new GameObject("MazeGen");
            gameManager.AddComponent<MazeGenerator>();
        }
    }

    public void EasyMode()
    {
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.SetMazeSize(new Vector2Int(15, 15));
            SceneManager.LoadScene("MazeGameScene");
        }
    }

    public void MediumMode()
    {
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.SetMazeSize(new Vector2Int(25, 25));
            SceneManager.LoadScene("MazeGameScene");
        }
    }

    public void HardMode()
    {
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.SetMazeSize(new Vector2Int(35, 35));
            SceneManager.LoadScene("MazeGameScene");
        }
    }
}
