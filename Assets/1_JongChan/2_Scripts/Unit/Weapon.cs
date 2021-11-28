using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 30f;
    
    private Player player;
    private Collider2D collider;
    private bool isSkill1;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject atkHitEffect;
    [SerializeField] private GameObject skill1Effect;
    [SerializeField] private GameObject skill1HitEffect;
    [SerializeField] private GameObject skill2HitEffect;
    
    private void Start()
    {
        transform.parent.TryGetComponent(out player);
        TryGetComponent(out collider);
    }

    public void Attack()
    {
        StartCoroutine(Co_Attack());
    }
    
    public void Skill1()
    {
        Instantiate(skill1Effect, transform.position + new Vector3(player.transform.localScale.x > 0 ? 5 : -5, 0,0), Quaternion.identity);
        StartCoroutine(Co_Skill1());
    }

    public void Skill2()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 0 : 180));
    }
    
    IEnumerator Co_Attack()
    {
        collider.enabled = true;
        yield return YieldCache.WaitForSeconds(0.1f);
        collider.enabled = false;
        yield return null;
    }

    IEnumerator Co_Skill1()
    {
        collider.enabled = true;
        isSkill1 = true;
        
        for (int i = 0; i < 10; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            player.transform.Translate((player.transform.localScale.x < 0 ? -10 : 10) * 10 * Time.deltaTime, 0, 0);
        }

        collider.enabled = false;
        isSkill1 = false;
        yield return null;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Unit enemy) && enemy.kind == Unit.Kind.Monster)
        {
            enemy.OnHit(player, (int)damage);
            
            if (isSkill1)
            {
                Instantiate(skill1HitEffect, other.transform.position, Quaternion.Euler(0,0,player.localScale.z > 0 ? 0 : 180));
            }
            else
            {
                Instantiate(atkHitEffect, other.transform.position, Quaternion.Euler(0, 0, player.localScale.z > 0 ? 0 : 180));
            }
        }
    }
}
