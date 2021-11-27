using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 30f;
    
    private Player player;
    
    private void Start()
    {
        transform.parent.TryGetComponent(out player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Unit enemy) && enemy.kind == Unit.Kind.Monster)
        {
            enemy.OnHit(player, (int)damage);
        }
    }
}
