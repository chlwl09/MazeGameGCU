using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector2Int mazeSize = new Vector2Int(25, 25); // �⺻ �̷� ũ��

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMazeSize(Vector2Int newSize)
    {
        mazeSize = newSize;
    }

    public Vector2Int GetMazeSize()
    {
        return mazeSize;
    }
}
