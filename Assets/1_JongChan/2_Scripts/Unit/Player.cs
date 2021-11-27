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

    [Header("타이머")] [SerializeField] private float dashTimer = 1;
    [SerializeField] private float dashInterval = 1;
    [SerializeField] private float skill1Timer = 10;
    [SerializeField] private float skill1Interval = 10;
    [SerializeField] private float skill2Timer = 20;
    [SerializeField] private float skill2Interval = 20;

    //내부
    private Collider2D weaponcollider;
    private Animator anim;
    Dictionary<KeyCode, Action> keyDictionary;
    
    protected override void Start()
    {
        base.Start();

        keyDictionary = new Dictionary<KeyCode, Action>
        {
            {KeyCode.LeftControl, Attack},
            {KeyCode.Space, Jump},
            {KeyCode.X, Dash},
            {KeyCode.A, Skill_1},
            {KeyCode.S, Skill_2}
        };

        transform.Find("Weapon").TryGetComponent(out weaponcollider);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        Timer();
        PlayerMove();
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
        
        if(Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }
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
            spriteRenderer.flipX = true;
            Horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            Horizontal = 1;
        }
        
        rigidbody2D.AddForce(Vector2.right * Horizontal, ForceMode2D.Impulse);

        if (rigidbody2D.velocity.x > moveSpeed)
        {
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        }
        else if (rigidbody2D.velocity.x < moveSpeed * (-1))
        {
            rigidbody2D.velocity = new Vector2(moveSpeed * (-1), rigidbody2D.velocity.y);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);
        }
    }

    private void Jump()
    {
        if (curJump >= maxJump)
            return;

        curJump++;

        if (curJump == 1)
        {
            anim.SetTrigger("IsJump1");
        }

        else if (curJump == 2)
        {
            anim.SetTrigger("IsJump2");
        }

        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(Co_Attack());
        }
    }

    private void Dash()
    {
        if (dashTimer < dashInterval) return;
        dashTimer = 0;
        
        StartCoroutine(Co_Dash());
    }

    private void Skill_1()
    {
        if (skill1Timer < skill1Interval) return;
        skill1Timer = 0;
        
        StartCoroutine(Co_Skill_1());
    }

    private void Skill_2()
    {
        if (skill2Timer < skill2Interval) return;
        skill2Timer = 0;
        
        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, spriteRenderer.flipX ? 180 : 0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ground")
        {
            curJump = 0;
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
    
    IEnumerator Co_Dash()
    {
        isDash = true;

        for (int i = 0; i < 5; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            transform.position +=
                new Vector3((spriteRenderer.flipX ? -moveSpeed : moveSpeed) * 10 * Time.deltaTime, 0, 0);
        }

        isDash = false;
    }
    
    IEnumerator Co_Skill_1()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            transform.position +=
                new Vector3((spriteRenderer.flipX ? -moveSpeed : moveSpeed) * 10 * Time.deltaTime, 0, 0);
        }
    }
}