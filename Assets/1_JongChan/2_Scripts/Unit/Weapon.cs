using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 30f;
    
    private Player player;
    private Collider2D collider;
    
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
        StartCoroutine(Co_Skill());
    }
    
    IEnumerator Co_Attack()
    {
        collider.enabled = true;
        yield return YieldCache.WaitForSeconds(0.1f);
        collider.enabled = false;
        yield return null;
    }

    IEnumerator Co_Skill()
    {
        collider.enabled = true;
        
        for (int i = 0; i < 10; i++)
        {
            yield return YieldCache.WaitForSeconds(0.01f);
            player.transform.position +=
                new Vector3((transform.localScale.x < 0 ? -10 : 10) * 10 * Time.deltaTime, 0, 0);
        }

        collider.enabled = false;
        yield return null;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Unit enemy) && enemy.kind == Unit.Kind.Monster)
        {
            enemy.OnHit(player, (int)damage);
        }
    }
}
