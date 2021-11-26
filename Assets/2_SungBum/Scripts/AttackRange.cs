using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy.GetComponent<KnifeEnemy>().PlayerDis < Enemy.GetComponent<KnifeEnemy>().MaxPlayerDis)
        {
            if (Enemy.GetComponent<KnifeEnemy>().TargetMove > 0)
            {
                this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.75f, 0.0f);
            }

            else
            {
                this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.75f, 0.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<KnifeEnemy>().AttackTiming = true;

            if(Enemy.gameObject.name == "KnifeEnemy")
            {
                //공격
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;

            if (Enemy.gameObject.name == "KnifeEnemy")
            {
                //다시 움직임
            }
        }
    }
}
