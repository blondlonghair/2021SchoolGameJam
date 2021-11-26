using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Player : Unit
{
    private float moveSpeed;
    private float jumpPower;
    private int isjump;
    
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Move();
    }
    
    void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonUp("Horizontal"))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.normalized.x * 0f, rigidbody2D.velocity.y);
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

        if (isjump >= 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isjump == 2)
                {
                    // anim.SetTrigger("isJump");
                }

                else if (isjump == 1)
                {
                    // anim.SetTrigger("isDoubleJump");
                }
                rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                isjump--;
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            // anim.SetBool("isRun", true);
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            // anim.SetBool("isRun", true);
        }

        else
        {
            // anim.SetBool("isRun", false);
        }
    }

    private void Skill1()
    {
        
    }
    
    
}
