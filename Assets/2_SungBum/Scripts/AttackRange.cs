using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    GameObject Enemy; // 부모 객체

    public float AttackDel = 0; // 현재 공격 딜레이
    public float BaseAttackDel = 0.45f; // 기본 공격 딜레이
    public float GunDelPlus = 0.2f; // 원거리 딜레이 추가량

    bool Attack = false; // 공격 가능 판정

    public int BulletDmg = 20; // 총 딜
    public int BulletPower = 15; // 총알 속도
    public int KnifeDmg = 30; // 근접 딜

    float dy;
    float dx;
    Transform playerPos; // 원거리용 플레이어 위치
    float rotateDg; // 총알 각도
    public GameObject PFBullet; // 원거리용 

    private void Awake()
    {
        Enemy = transform.parent.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
        {
            AttackDel = BaseAttackDel;
        }

        else if (Enemy.gameObject.tag == "GunEnemy") // 원거리 몹 일때
        {
            AttackDel = BaseAttackDel + GunDelPlus;
        }

        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Enemy.GetComponent<KnifeEnemy>().PlayerAttackDmg = KnifeDmg;
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

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Enemy.GetComponent<KnifeEnemy>().AttackTiming = true;

            if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
            {
                //Debug.Log("hi");
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime; // 쿨감소

                if (AttackDel <= 0.0f) // 쿨돌면 공격
                {
                    AttackPly();
                    AttackDel = BaseAttackDel;
                    StartCoroutine("KnifeEnemyAtk");
                }

                else if (AttackDel <= 0.25f)
                    AtkReady();
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // 원거리 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = true;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime;

                float dy = playerPos.position.y - transform.position.y;
                float dx = playerPos.position.x - transform.position.x;
                rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // 총알 각도 변경

                Vector2 dir = playerPos.position - transform.position; // 플레이어 위치로 발사
                dir.Normalize();

                if (AttackDel <= 0.0f) // 총알 발사
                {
                    AttackPly();
                    AttackDel = BaseAttackDel + GunDelPlus;
                    GameObject Bullet = Instantiate(PFBullet, transform.position, Quaternion.Euler(0f, 0f, rotateDg));
                    Bullet.GetComponent<BulletDamege>(). BulletDg = BulletDmg;
                    Bullet.GetComponent<Rigidbody2D>().AddForce(dir * BulletPower, ForceMode2D.Impulse);
                }

                else if(AttackDel <= 0.51f)
                    AtkReady();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 공격 범위 벗어났을 때
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GoRun();
            if (Enemy.gameObject.tag == "KnifeEnemy") // 근접 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = BaseAttackDel;
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // 근접 몹 일때
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = BaseAttackDel + GunDelPlus;
            }
        }
    }    

    IEnumerator KnifeEnemyAtk()
    {
        Enemy.GetComponent<KnifeEnemy>().PlayerAttack(); // 근접 공격 성공했을 때 부모 스크립트에 있는 공격 함수 실행
        yield return YieldCache.WaitForSeconds(0.01f);
    }

    void AtkReady()
    {
        Enemy.GetComponent <KnifeEnemy>().animator.SetBool("AtkReady",true);
        Enemy.GetComponent <KnifeEnemy>().animator.SetBool("Attack",false);
        Enemy.GetComponent <KnifeEnemy>().animator.SetBool("Run",false);
    }

    void AttackPly()
    {
        Enemy.GetComponent<KnifeEnemy>().animator.SetBool("Attack", true);
    }

    void GoRun()
    {
        Enemy.GetComponent<KnifeEnemy>().animator.SetBool("AtkReady", false);
        Enemy.GetComponent<KnifeEnemy>().animator.SetBool("Attack", false);
        Enemy.GetComponent<KnifeEnemy>().animator.SetBool("Run", true);
    }
}
