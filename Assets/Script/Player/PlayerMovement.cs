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
    private Animator animator; // Animator ������Ʈ ����

    void Start()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    void Update()
    {
        UpdateAnimation(); // �ִϸ��̼� ������Ʈ
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

            // �÷��̾ ������ �� playerSoundEffect ������Ʈ�� Ȱ��ȭ
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
            animator.SetBool("isJumping", true); // ���� ���� ����
        }
    }

    // �浹 �Լ�
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("isJumping", false); // ���� ���� ����
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
