using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    GameObject Enemy; // �θ� ��ü

    public float AttackDel = 0; // ���� ���� ������
    public float BaseAttackDel = 0.7f; // �⺻ ���� ������
    public float GunDelPlus = 0.2f; // ���Ÿ� ������ �߰���

    bool Attack = false; // ���� ���� ����

    public int BulletDmg = 20; // �� ��
    public int BulletPower = 15; // �Ѿ� �ӵ�
    public int KnifeDmg = 30; // ���� ��

    float dy;
    float dx;
    Transform playerPos; // ���Ÿ��� �÷��̾� ��ġ
    float rotateDg; // �Ѿ� ����
    public GameObject PFBullet; // ���Ÿ��� �Ѿ�

    private void Awake()
    {
        Enemy = transform.parent.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Enemy.gameObject.tag == "KnifeEnemy") // ���� �� �϶�
        {
            AttackDel = BaseAttackDel;
        }

        else if (Enemy.gameObject.tag == "GunEnemy") // ���Ÿ� �� �϶�
        {
            AttackDel = BaseAttackDel + GunDelPlus;
        }

        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Enemy.GetComponent<KnifeEnemy>().PlayerAttackDmg = KnifeDmg;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.gameObject.tag == "KnifeEnemy") // ���� �� �϶�
        {
            if (Enemy.GetComponent<KnifeEnemy>().PlayerDis < Enemy.GetComponent<KnifeEnemy>().MaxPlayerDis)
            {
                if (Enemy.GetComponent<KnifeEnemy>().TargetMove > 0)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.75f, 0.0f); // �÷��̾� ��ġ�� ���� �ݶ��̴� ��ġ ����
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

            if (Enemy.gameObject.tag == "KnifeEnemy") // ���� �� �϶�
            {
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime; // �𰨼�

                if (AttackDel <= 0.0f) // �𵹸� ����
                {
                    AttackDel = BaseAttackDel;
                    StartCoroutine("KnifeEnemyAtk");
                }
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // ���Ÿ� �� �϶�
            {
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 0.0f;
                AttackDel -= 0.3f * Time.deltaTime;

                float dy = playerPos.position.y - transform.position.y;
                float dx = playerPos.position.x - transform.position.x;
                rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg; // �Ѿ� ���� ����

                Vector2 dir = playerPos.position - transform.position; // �÷��̾� ��ġ�� �߻�
                dir.Normalize();

                if (AttackDel <= 0.0f) // �Ѿ� �߻�
                {
                    AttackDel = BaseAttackDel + GunDelPlus;
                    GameObject Bullet = Instantiate(PFBullet, transform.position, Quaternion.Euler(0f, 0f, rotateDg));
                    Bullet.GetComponent<BulletDamege>(). BulletDg = BulletDmg;
                    Bullet.GetComponent<Rigidbody2D>().AddForce(dir * BulletPower, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // ���� ���� ����� ��
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enemy.gameObject.tag == "KnifeEnemy") // ���� �� �϶�
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = BaseAttackDel;
            }

            else if (Enemy.gameObject.tag == "GunEnemy") // ���� �� �϶�
            {
                Enemy.GetComponent<KnifeEnemy>().AttackTiming = false;
                Enemy.GetComponent<KnifeEnemy>().MoveSpeed = 2.0f;
                AttackDel = BaseAttackDel + GunDelPlus;
            }
        }
    }    

    IEnumerator KnifeEnemyAtk()
    {
        Enemy.GetComponent<KnifeEnemy>().PlayerAttack(); // ���� ���� �������� �� �θ� ��ũ��Ʈ�� �ִ� ���� �Լ� ����
        yield return YieldCache.WaitForSeconds(0.01f);
    }
}
