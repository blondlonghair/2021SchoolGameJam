using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Player : Unit
{
    [Header("Player")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int curJump;
    [SerializeField] private int maxJump;
    [SerializeField] private bool isDash;

    [Header("ref")] [SerializeField] private GameObject bullet;

    private float dashTimer = 1;
    private float dashInterval = 1;
    private Collider2D weaponcollider;
    private float skill1Timer = 10;
    private float skill1Interval = 10;
    private float skill2Timer = 20;
    private float skill2Interval = 20;

    protected override void Start()
    {
        base.Start();
        weaponcollider = transform.Find("Weapon").GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        Timer();
        PlayerMove();
        
        if (Input.GetKeyDown(KeyCode.A) && skill1Timer > skill1Interval)
        {
            Skill_1();
            skill1Timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.S) && skill2Timer > skill2Interval)
        {
            Skill_2();
            skill2Timer = 0;
        }
    }

    void Timer()
    {
        dashTimer += Time.deltaTime;
        skill1Timer += Time.deltaTime;
        skill2Timer += Time.deltaTime;
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
        float Horizontal = 0;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Horizontal = -1;
            spriteRenderer.flipX = true;
        }            
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Horizontal = 1;
            spriteRenderer.flipX = false;
        }

        rigidbody2D.AddForce(Vector2.right * Horizontal, ForceMode2D.Impulse);

        if (rigidbody2D.velocity.x > moveSpeed)
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        else if (rigidbody2D.velocity.x < moveSpeed * (-1))
            rigidbody2D.velocity = new Vector2(moveSpeed * (-1), rigidbody2D.velocity.y);

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);
        }
        
        if (curJump >= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (curJump == 1)
                {
                    // anim.SetTrigger("isDoubleJump");
                }
                rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curJump--;
            }
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
        isDash = true;
        
        for (int i = 0; i < 5; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            transform.position += new Vector3((spriteRenderer.flipX ? -moveSpeed : moveSpeed) * 10 * Time.deltaTime, 0, 0);
        }

        isDash = false;
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
        Instantiate(bullet, transform.position, spriteRenderer.flipX ? quaternion.Euler(0, 0, 180) : quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ground")
        {
            curJump = maxJump;
        }
    }
}