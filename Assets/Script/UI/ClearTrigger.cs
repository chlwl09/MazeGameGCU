using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    public GameObject clearUI;  // 나타낼 UI 오브젝트
    public GameObject playerEffectSound;
    private GameClearUIManager gameClearUIManager;

    private void Start()
    {
        gameClearUIManager = FindObjectOfType<GameClearUIManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 클리어 UI 활성화
            clearUI.SetActive(true);
            playerEffectSound.SetActive(false);

            Time.timeScale = 0; //시간 멈추기

            gameClearUIManager.OnGameClear();
        }
    }
}
