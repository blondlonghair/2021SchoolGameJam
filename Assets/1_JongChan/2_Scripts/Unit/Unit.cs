using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor
{
    public enum Kind
    {
        None,
        Player,
        Monster
    }

    public Kind kind; //인스팩터에서 세팅

    [Header("Hp")] public float curHp;
    public float maxHp;

    protected Rigidbody2D rigidbody2D;
    protected Collider2D collider;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnHit(Unit attacker, int power)
    {
        curHp -= power;

        if (curHp <= 0)
        {
            OnDead(attacker);
        }
    }

    public virtual void OnDead(Unit from)
    {
        Destroy(gameObject);
    }
}