using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    GameObject Enemy; // 부모 객체
    float AttackDel = 0.7f; // 공격 딜레이
    bool Attack = false; // 공격 딜레이

    float dy;
    float dx;
    Transform playerPos; // 원거리용 플레이어 위치
    float rotateDg; // 총알 각도
    public GameObject PFBullet; // 원거리용 총알

    // Start is called before the first frame update
    void Start()
    {
        Enemy = transform.parent.gameObject;
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
        {
            if (Enemy.GetComponent<KnifeEnemy>().PlayerDis < Enemy.GetComponent<KnifeEnemy>().MaxPlayerDis)
            {
                if (Enemy.GetComponent<KnifeEnemy>().TargetMove > 0)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.75f, 0.0f); // 플레이어 위치에 따라 콜라이더 위치 변경
                }

                else
                {
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.75f, 0.0f);
                }
            }
        }

        else if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
        {
            if (Enemy.GetComponent<KnifeEnemy>().PlayerDis < Enemy.GetComponent<KnifeEnemy>().MaxPlayerDis)
            {
                if (Enemy.GetComponent<KnifeEnemy>().TargetMove > 0)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.75f, 0.0f); // 플레이어 위치에 따라 콜라이더 위치 변경
                }

                else
                {
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.75f, 0.0f);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<KnifeEnemy>().AttackTiming = true;

            if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime; // 쿨감소

                if (AttackDel <= 0.0f) // 쿨돌면 공격
                {
                    AttackDel = 0.7f;
                    StartCoroutine("KnifeEnemyAtk");
                }
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // 원거리 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime;

                float dy = playerPos.position.y - transform.position.y;
                float dx = playerPos.position.x - transform.position.x;
                rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // 총알 각도 변경

                Vector2 dir = playerPos.position - transform.position; // 플레이어 위치로 발사
                dir.Normalize();

                if (AttackDel <= 0.0f) // 총알 발사
                {
                    AttackDel = 0.4f;
                    GameObject Bullet = Instantiate(PFBullet, transform.position, Quaternion.Euler(0f, 0f, rotateDg));
                    Bullet.GetComponent<Rigidbody2D>().AddForce(dir * 10, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 공격 범위 벗어났을 때
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = 0.7f;
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // 근접 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = 0.7f;
            }
        }
    }    

    IEnumerator KnifeEnemyAtk()
    {
        Enemy.GetComponent<KnifeEnemy>().PlayerAttack(); // 근접 공격 성공했을 때 부모 스크립트에 있는 공격 함수 실행
        yield return YieldCache.WaitForSeconds(0.01f);
    }
}
