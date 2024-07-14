using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    public GameObject clearUI;  // ��Ÿ�� UI ������Ʈ
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
            // Ŭ���� UI Ȱ��ȭ
            clearUI.SetActive(true);
            playerEffectSound.SetActive(false);

            Time.timeScale = 0; //�ð� ���߱�

            gameClearUIManager.OnGameClear();
        }
    }
}
