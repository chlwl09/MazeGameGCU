using System.Collections;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public GameObject arrowTarget; // 화살표를 참조하는 변수 추가

    private void Start()
    {
        arrowTarget = GameObject.FindWithTag("Arrow");
        if (arrowTarget == null)
        {
            Debug.LogError("화살표 태그 없음.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 충돌");

            if (arrowTarget != null)
            {
                Debug.Log("화살표 켜짐");
                StartCoroutine(ShowArrowTarget(arrowTarget));
            }
            else
            {
                Debug.LogError("화살표 태그를 가진 오브젝트가 없음.");
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator ShowArrowTarget(GameObject arrowTarget)
    {
        arrowTarget.SetActive(true);
        yield return new WaitForSeconds(3f);
        arrowTarget.SetActive(false);
        Debug.Log("화살표 꺼짐");
    }
}
