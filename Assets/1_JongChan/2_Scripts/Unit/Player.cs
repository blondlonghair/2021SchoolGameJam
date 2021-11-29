using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("타이머")] 
    [SerializeField] public float dashTimer = 1;
    [SerializeField] public float dashInterval = 1;
    [SerializeField] public float atkTimer = 0.5f;
    [SerializeField] public float atkInterval = 0.5f;
    [SerializeField] public float skill1Timer = 10;
    [SerializeField] public float skill1Interval = 10;
    [SerializeField] public float skill2Timer = 20;
    [SerializeField] public float skill2Interval = 20;

    [Header("참조")] 
    [SerializeField] private GameObject dashEffect;
    
    //내부
    private Weapon weapon;
    private Animator anim;
    Dictionary<KeyCode, Action> keyDictionary;
    
    protected override void Start()
    {
        base.Start();

        keyDictionary = new Dictionary<KeyCode, Action>
        {
            {KeyCode.LeftAlt, Jump},
            {KeyCode.X, Dash},
            {KeyCode.A, Skill_1},
            {KeyCode.S, Skill_2}
        };

        transform.Find("Weapon").TryGetComponent(out weapon);
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
        atkTimer += Time.deltaTime;
    }

    void PlayerMove()
    {
        Move();

        Attack();

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
        
        if (rigidbody2D.velocity.x > moveSpeed)
        {
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        }
        else if (rigidbody2D.velocity.x < moveSpeed * (-1))
        {
            rigidbody2D.velocity = new Vector2(moveSpeed * (-1), rigidbody2D.velocity.y);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftControl))
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            rigidbody2D.AddForce(Vector2.left * Time.deltaTime * 100, ForceMode2D.Impulse);
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftControl))
        {
            gameObject.transform.localScale = new Vector3(1,1,1);
            rigidbody2D.AddForce(Vector2.right * Time.deltaTime * 100, ForceMode2D.Impulse);
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);
            anim.SetTrigger("isIdle");
        }
    }

    private void Jump()
    {
        if (curJump >= maxJump)
            return;

        curJump++;

        if (curJump == 1)
        {
            anim.SetInteger("isJump", curJump);
        }

        else if (curJump == 2)
        {
            Instantiate(dashEffect, transform.position, Quaternion.Euler(0,0,90));
            anim.SetInteger("isJump", curJump);
        }

        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("isAttack", true);
        }
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (atkTimer < atkInterval) return;
            atkTimer = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigidbody2D.AddForce(Vector2.right * -100, ForceMode2D.Impulse);
                weapon.Attack();
                print("LeftArrowAtk");
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rigidbody2D.AddForce(Vector2.right * 100, ForceMode2D.Impulse);
                weapon.Attack();
                print("RightArrowAtk");
            }

            else
            {
                weapon.Attack();
                print("JustAtk");
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            atkTimer = 0.5f;
            anim.SetBool("isAttack", false);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && curJump > 0)
        {
            anim.SetTrigger("isJumpAttack");
            weapon.Attack();
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
        
        anim.SetTrigger("isSkill1");
        weapon.Skill1();
    }

    private void Skill_2()
    {
        if (skill2Timer < skill2Interval) return;
        skill2Timer = 0;
        
        anim.SetTrigger("isSkill2");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Tilemap")
        {
            curJump = 0;
            anim.SetInteger("isJump", curJump);
        }
    }

    public void Skill2_Shot()
    {
        weapon.Skill2();
    }

    public void Attack_CheckPosition()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            localScale = new Vector3(-1, 1, 1);
        }
        
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            localScale = new Vector3(1, 1, 1);
        }
    }
    
    IEnumerator Co_Dash()
    {
        isDash = true;
        anim.SetBool("isDash", isDash);
        Instantiate(dashEffect, transform.position + new Vector3(transform.localScale.x > 0 ? 0.5f : -0.5f, 0,0), Quaternion.identity);

        for (int i = 0; i < 3; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            
            rigidbody2D.AddForce(Vector2.right * (transform.localScale.x > 0 ? 100 : -100) * Time.deltaTime * 10, ForceMode2D.Impulse);
        }

        yield return YieldCache.WaitForSeconds(0.1f);
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);

        isDash = false;
        anim.SetBool("isDash", isDash);
    }

    public override void OnHit(Unit attacker, int power)
    {
        GameManager.In.HitEffect();
        base.OnHit(attacker, power);
    }
}