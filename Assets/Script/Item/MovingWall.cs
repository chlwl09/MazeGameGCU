using System.Collections;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public float moveDistance = -1.0f; // 이동할 거리
    public float waitTime = 3.0f; // 대기 시간

    private Vector3 originalPosition; // 원래 위치
    private bool isMovingUp = true; // 올라가고 있는지 여부

    void Start()
    {
        // 초기 위치 설정 (Y값을 1로 설정)
        originalPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.position = originalPosition; // 오브젝트의 위치를 초기 위치로 설정
        StartCoroutine(MoveWall()); // 코루틴 시작
    }

    IEnumerator MoveWall()
    {
        while (true)
        {
            // 목표 위치 계산
            Vector3 targetPosition = originalPosition + (isMovingUp ? new Vector3(0, moveDistance, 0) : new Vector3(0, -moveDistance, 0));

            // 오브젝트 위치 이동
            transform.position = targetPosition;

            // 3초 대기
            yield return new WaitForSeconds(waitTime);

            // 방향 전환
            isMovingUp = !isMovingUp;
        }
    }
}
