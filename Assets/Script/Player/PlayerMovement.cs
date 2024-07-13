using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10.0f;
    public float rotateSpeed = 10.0f;
    public float jumpForce = 1.0f;
    private bool isGround = true;
    public GameObject playerSoundEffect;

    private Rigidbody body;
    private float h, v;
    private Animator animator; // Animator 컴포넌트 참조

    void Start()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    void Update()
    {
        UpdateAnimation(); // 애니메이션 업데이트
    }

    void FixedUpdate()
    {
        Move();
        //Jump();
    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            transform.position += dir * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);

            // 플레이어가 움직일 때 playerSoundEffect 오브젝트를 활성화
            if (!playerSoundEffect.activeInHierarchy)
            {
                playerSoundEffect.SetActive(true);
            }

            animator.SetBool("isMoving", true);
        }
        else
        {
            if (playerSoundEffect.activeInHierarchy)
            {
                playerSoundEffect.SetActive(false);
            }

            animator.SetBool("isMoving", false);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
            animator.SetBool("isJumping", true); // 점프 상태 설정
        }
    }

    // 충돌 함수
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("isJumping", false); // 점프 상태 해제
        }
    }

    void UpdateAnimation()
    {
        if (isGround)
        {
            animator.SetBool("isJumping", false);

            if (h == 0 && v == 0)
            {
                animator.SetBool("isMoving", false);
            }
            else
            {
                animator.SetBool("isMoving", true);
            }
        }
        else
        {
            animator.SetBool("isJumping", true);
        }
    }
}
