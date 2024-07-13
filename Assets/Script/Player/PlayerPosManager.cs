using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosManager : MonoBehaviour
{
    public GameObject player;
    public GameObject GameClear;

    private void Start()
    {
        if (GameManager.Instance != null && player != null)
        {
            player.transform.position = GameManager.Instance.GetPlayerStartPosition();
            if (GameClear != null)
            {
                GameClear.transform.position = GameManager.Instance.GetGameClearPosition();
            }
        }
    }
}