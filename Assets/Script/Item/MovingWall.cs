using System.Collections;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public float moveDistance = -1.0f; // �̵��� �Ÿ�
    public float waitTime = 3.0f; // ��� �ð�

    private Vector3 originalPosition; // ���� ��ġ
    private bool isMovingUp = true; // �ö󰡰� �ִ��� ����

    void Start()
    {
        // �ʱ� ��ġ ���� (Y���� 1�� ����)
        originalPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.position = originalPosition; // ������Ʈ�� ��ġ�� �ʱ� ��ġ�� ����
        StartCoroutine(MoveWall()); // �ڷ�ƾ ����
    }

    IEnumerator MoveWall()
    {
        while (true)
        {
            // ��ǥ ��ġ ���
            Vector3 targetPosition = originalPosition + (isMovingUp ? new Vector3(0, moveDistance, 0) : new Vector3(0, -moveDistance, 0));

            // ������Ʈ ��ġ �̵�
            transform.position = targetPosition;

            // 3�� ���
            yield return new WaitForSeconds(waitTime);

            // ���� ��ȯ
            isMovingUp = !isMovingUp;
        }
    }
}
