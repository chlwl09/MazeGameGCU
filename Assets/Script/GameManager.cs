using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Vector2Int mazeSize;
    private Vector3 playerStartPosition;

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

    public void SetMazeSize(Vector2Int size)
    {
        mazeSize = size;
    }

    public Vector2Int GetMazeSize()
    {
        return mazeSize;
    }

    public void SetPlayerStartPosition(Vector3 position)
    {
        playerStartPosition = position;
    }

    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPosition;
    }
}
