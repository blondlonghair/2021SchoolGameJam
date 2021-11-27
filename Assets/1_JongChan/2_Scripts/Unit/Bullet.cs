using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        if (transform.parent.TryGetComponent<Player>(out var parent))
        {
            player = parent;
        }
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * 10, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Unit>(out var unit))
        {
            if (unit.kind == Unit.Kind.Monster)
            {
                unit.OnHit(null, 30);
            }
        }
    }
}