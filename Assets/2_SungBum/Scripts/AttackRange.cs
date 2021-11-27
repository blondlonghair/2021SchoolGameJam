using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    GameObject Enemy;
    float AttackDel = 0.7f; // °ø°Ý µô·¹ÀÌ
    bool Attack = false; // °ø°Ý µô·¹ÀÌ

    Player player = null;

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<KnifeEnemy>().AttackTiming = true;

            if (Enemy.gameObject.tag == "KnifeEnemy") // ±ÙÁ¢ ¸÷ ÀÏ¶§
            {
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime;

                if(AttackDel <= 0.0f)
                {
                    AttackDel = 0.7f;
                    StartCoroutine("KnifeEnemyAtk");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enemy.gameObject.tag == "KnifeEnemy") // ±ÙÁ¢ ¸÷ ÀÏ¶§
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = 0.7f;
            }
        }
    }    

    IEnumerator KnifeEnemyAtk()
    {
        Enemy.GetComponent<KnifeEnemy>().PlayerAttack();
        yield return YieldCache.WaitForSeconds(0.01f);
    }
}
