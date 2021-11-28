using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    
    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * 20, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Unit unit) && unit.kind == Unit.Kind.Monster)
        {
            unit.OnHit(new Player(), 30);
            Instantiate(effect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
