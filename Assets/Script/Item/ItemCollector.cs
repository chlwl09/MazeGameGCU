using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private GameObject arrowTarget;

    private void Start()
    {
        arrowTarget = GameObject.FindWithTag("Arrow");
        if (arrowTarget == null)
        {
            Debug.LogError("ȭ��ǥ �±� ����.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �浹");

            if (arrowTarget != null)
            {
                Debug.Log("ȭ��ǥ ����");
                StartCoroutine(ShowArrowTarget(arrowTarget));
            }
            else
            {
                Debug.LogError("ȭ��ǥ �±׸� ���� ������Ʈ�� ���� 2.");
            }
        }
    }

    private IEnumerator ShowArrowTarget(GameObject arrowTarget)
    {
        arrowTarget.SetActive(true);
        yield return new WaitForSeconds(3f);
        arrowTarget.SetActive(false);
        Debug.Log("ȭ��ǥ ����");
    }
}