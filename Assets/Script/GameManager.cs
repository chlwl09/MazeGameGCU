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
            Debug.Log("GameManager Instance created");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("GameManager Instance already exists, destroying new instance");
            Destroy(gameObject);
        }
    }

    public void SetMazeSize(Vector2Int size)
    {
        Debug.Log("Setting maze size to " + size);
        mazeSize = size;
    }

    public Vector2Int GetMazeSize()
    {
        return mazeSize;
    }

    public void SetPlayerStartPosition(Vector3 position)
    {
        Debug.Log("Setting player start position to " + position);
        playerStartPosition = position;
    }

    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPosition;
    }
}
