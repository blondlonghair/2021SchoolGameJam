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
    private Rigidbody2D playerRigidbody2D;
    private bool isSkill1;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject atkHitEffect;
    [SerializeField] private GameObject skill1Effect;
    [SerializeField] private GameObject skill1HitEffect;
    [SerializeField] private GameObject skill2HitEffect;
    
    private void Start()
    {
        transform.parent.TryGetComponent(out player);
        transform.parent.TryGetComponent(out playerRigidbody2D);
        TryGetComponent(out collider);
    }

    public void Attack()
    {
        StartCoroutine(Co_Attack());
    }
    
    public void Skill1()
    {
        Instantiate(skill1Effect, transform.position + new Vector3(player.transform.localScale.x > 0 ? 3 : -3,0), Quaternion.Euler(0, player.transform.localScale.x > 0 ? 0 : 180, 0));
        StartCoroutine(Co_Skill1());
    }

    public void Skill2()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, player.transform.localScale.x > 0 ? 0 : 180));
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
        
        for (int i = 0; i < CheckDistance(5); i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            player.transform.Translate(new Vector3((player.transform.localScale.x > 0 ? 1 : -1), 0, 0));
        }
        
        yield return YieldCache.WaitForSeconds(0.1f);
        
        collider.enabled = false;
        isSkill1 = false;
        yield return null;
    }
    
    int CheckDistance(int time)
    {
        int distance = 0;

        for (int i = 0; i < time; i++)
        {
            RaycastHit2D[] ray = Physics2D.RaycastAll(player.transform.position, player.transform.localScale.x > 0 ? Vector2.right : Vector2.left, distance + 0.5f);

            foreach (var raycastHit2D in ray)
            {
                print(raycastHit2D.transform.name);
            }
            
            foreach (var hit2D in ray)
            {
                if (hit2D.transform.name == "Tilemap")
                {
                    print(distance);
                    return distance;
                }
            }

            distance++;
        }

        print(distance);
        return distance;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Unit enemy) && enemy.kind == Unit.Kind.Monster)
        {
            
            if (isSkill1)
            {
                enemy.OnHit(player, (int)damage * 2);
                Instantiate(skill1HitEffect, other.transform.position, Quaternion.Euler(0,0,player.localScale.z > 0 ? 0 : 180));
            }
            else
            {
                enemy.OnHit(player, (int)damage);
                Instantiate(atkHitEffect, other.transform.position, Quaternion.Euler(0, 0, player.localScale.z > 0 ? 0 : 180));
            }
        }
    }
}
