using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : Unit
{
    [Header("Player")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJump;
    [SerializeField] private int maxJump;
    [SerializeField] private bool isDash;
    [SerializeField] private float AttackRange;

    private float dashTimer = 0;
    private float dashInterval = 1;
    private Collider2D weaponcollider;

    protected override void Start()
    {
        base.Start();
        weaponcollider = transform.Find("Weapon").GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        dashTimer += Time.deltaTime;

        PlayerMove();
    }

    void PlayerMove()
    {
        Idle(); 
        Move();
        Dash();
        Attack();
    }

    private void Idle()
    {
        //Idle 애니메이션 추가
    }

    private void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");

        rigidbody2D.AddForce(Vector2.right * Horizontal, ForceMode2D.Impulse);

        if (rigidbody2D.velocity.x > moveSpeed)
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        else if (rigidbody2D.velocity.x < moveSpeed * (-1))
            rigidbody2D.velocity = new Vector2(moveSpeed * (-1), rigidbody2D.velocity.y);

        if (Input.GetButtonUp("Horizontal"))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);
        }
        
        if (curJump >= 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (curJump == 1)
                {
                    // anim.SetTrigger("isDoubleJump");
                }
                rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curJump--;
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false;
        }

        else
        {
            // anim.SetBool("isRun", false);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(Co_Attack());
        }
    }

    IEnumerator Co_Attack()
    {
        //공격 애니메이션 추가 할 곳
        weaponcollider.enabled = true;
        yield return YieldCache.WaitForSeconds(1f);
        weaponcollider.enabled = false;
        yield return null;
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.X) && dashTimer > dashInterval)
        {
            print("Dash");
            StartCoroutine(Co_Dash());
        }
    }

    IEnumerator Co_Dash()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            transform.position += new Vector3((spriteRenderer.flipX ? -moveSpeed : moveSpeed) * 10 * Time.deltaTime, 0, 0);
        }
    }

    private void Skill_1()
    {
        StartCoroutine(Co_Skill_1());
    }

    IEnumerator Co_Skill_1()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            transform.position += new Vector3((spriteRenderer.flipX ? -moveSpeed : moveSpeed) * 10 * Time.deltaTime, 0, 0);
        }
    }

    private void Skill_2()
    {
        
    }

    IEnumerator Co_Skill_2()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ground")
        {
            curJump = maxJump;
        }
    }
}