using System.Collections;
using UnityEngine;
public class ClearPosTarget : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        StartCoroutine(HideObjectDelayed());
    }

    void Update()
    {
        Vector3 vector = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(vector).normalized;
    }

    private IEnumerator HideObjectDelayed()
    {
        yield return new WaitForSeconds(0.001f);
        gameObject.SetActive(false);
        Debug.Log("¿ÀºêÁ§Æ® ¼û±è");
    }
}