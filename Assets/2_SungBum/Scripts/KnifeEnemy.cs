using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEnemy : Unit
{
    Vector3 BasePosMax;
    Vector3 BasePosMin;
    [SerializeField] Vector3 BasePos;
    Vector3 CurPos;
    Vector3 MaxPos;

    Player player = null;

    public float MoveSpeed = 2.0f;

    bool TargetChk = false;
    bool MoveCheck = false;
    bool BaseChk = false;
    
    public bool AttackTiming = false;

    int RanDir = 0;

    public float Waits = 0.6f;

    float BaseDis;
    public int MaxBaseDis = 4;

    GameObject Player = null;

    Vector2 PlayerPos;
    public float PlayerDis;
    public int MaxPlayerDis;
    
    public float TargetMove;

    [SerializeField] public int PlayerAttackDmg = 0;

    [SerializeField] public Animator animator;
    public bool GroundChk = true; // 공중부양 방지 변수
    float Waitcool = 0.0f; //  어그로 풀림 제한 시간

    public GameObject KnifeEffect; // 근거리 적 이펙트
    public GameObject EnemyDieEffect; // 근거리 적 이펙트

    private void Awake()
    {
        BasePosMax.x = this.gameObject.transform.position.x + 3.5f;
        BasePosMin.x = this.gameObject.transform.position.x - 3.5f;
        BasePos = this.gameObject.transform.position;

        CurPos = this.gameObject.transform.position;

        Player = GameObject.FindWithTag("Player");
        player = Player.GetComponent<Player>();

        if (this.gameObject.tag == "KnifeEnemy") // 근접 어그로 범위
            MaxPlayerDis = 4;

        else if(this.gameObject.tag == "GunEnemy") // 원거리 어그로 범위
            MaxPlayerDis = 8;

        animator = this.gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
        //Debug.Log(PlayerDis);
        CurPos = this.gameObject.transform.position;
        BaseDis = Vector3.Distance(BasePos, CurPos);
        BaseDis = Vector3.Distance(BasePos, CurPos);

        PlayerPos = Player.gameObject.transform.position;

        PlayerDis = Vector2.Distance(PlayerPos, CurPos);

        //Debug.Log(TargetChk);

        if (PlayerDis < MaxPlayerDis) // 어그로 끌렸을 때 
            TargetChk = true;

        else if(PlayerDis > MaxPlayerDis && TargetChk == true) // 어그로 풀렸을 때 다시 베이스 위치 잡아줌
        {
            //Debug.Log("TargetFalse");
            BasePos = CurPos;
            TargetChk = false;
        }

        if (Waitcool >= 0)
        {
            Waitcool -= 1 * Time.deltaTime;
        }
        //Debug.Log(BaseDis);
    }

        //private void OnCollisionEnter2D(Collision2D other)
        //{
        //    if (other.gameObject.tag == "Player")
        //    {
        //        if (waitcool <= 0.0f)
        //        {
        //            waitcool = 0.1f;
        //            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        //        }
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D other)
        //{
        //    if (other.gameObject.CompareTag("Player"))
        //    {
        //        if (waitcool <= 0.0f)
        //        {
        //            //Debug.Log("HH");
        //            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        //            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        //        }
        //    }
        //}

    public override void OnDead(Unit from) //에네미 죽음 이펙트
    {
        Instantiate(EnemyDieEffect,this.gameObject.transform.position,this.gameObject.transform.rotation);
        Destroy(gameObject);
    }

    public void EnemyMove()
    {
        if(GroundChk == false)
        {
            TargetMove = PlayerPos.x - CurPos.x;

            if (BasePos.x - this.gameObject.transform.position.x < 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                this.gameObject.transform.Translate(-MoveSpeed * 1.5f * Time.deltaTime, 0.0f, 0.0f);
            }

            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                this.gameObject.transform.Translate(MoveSpeed * 1.5f * Time.deltaTime, 0.0f, 0.0f);
            }

            StartCoroutine("TargetFalse");
        }

        else if (TargetChk == true && AttackTiming == false && GroundChk == true) // 媛??쒓굅由щ궡 player媛? ?덉쓣????吏곸씠怨? ?ъ젙 嫄곕━???덉쑝硫?硫덉땄
        {
            Run();
            //Debug.Log("Attack");
            TargetMove = PlayerPos.x - CurPos.x;

            if (GroundChk == true)
            {
                if (TargetMove < 0)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    this.gameObject.transform.Translate(-MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);
                }

                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    this.gameObject.transform.Translate(MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);
                }
            }
        }

        else if ((BaseDis > MaxBaseDis || BaseChk == true) && TargetChk == false && AttackTiming == false && GroundChk == true) //?ㅽ룿 ?꾩튂?먯꽌 硫?由??⑥뼱議뚯쓣?????먮━濡??뚯븘媛?
        {
            Walk();
            //Debug.Log("Target");
            BaseChk = true;
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, BasePos, 1.3f * Time.deltaTime); 

            if(BasePos.x - this.gameObject.transform.position.x < 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            if (BaseDis < 0.3) // 다시 베이스 위치로 왔을 때 랜덤 이동
            {
                GroundChk = true;
                BaseChk = false;
                RanDir = Random.Range(-1, 2);
            }
        }

        else if (RanDir == 0 && BaseChk == false && TargetChk == false && AttackTiming == false && GroundChk == true) // ?쒕뜡 ??吏곸엫 : 0?쇰븣 媛?留뚰엳
        {
            Wait();

            //Debug.Log("Wait");
            if (Waits > 0)
                Waits -= Time.deltaTime * 0.1f;

            else if(Waits <= 0)
            {
                Waits = 0.2f;
                RanDir = Random.Range(-1, 2);
            }
        }

        else if(RanDir == 1 && BaseChk == false && TargetChk == false && AttackTiming == false && GroundChk == true) // ?쒕뜡 ??吏곸엫 : 1?쇰븣 ?ㅻⅨ履?
        {
            Walk();
            //Debug.Log("Right");
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            if (MoveCheck == false) // 이동 위치 정해줌
            {
                Waits = 0.3f;
                MoveCheck = true;
                MaxPos.x = CurPos.x + 2.0f;
            }

            if (Waits > 0)
                Waits -= Time.deltaTime * 0.1f;

            if (CurPos.x < MaxPos.x)
            {
                StartCoroutine("RightMove");
            }

            else if(Waits < 0 && CurPos.x >= MaxPos.x) // 다 움직였을 때 다시 랜덤
            {
                RanDir = Random.Range(-1, 2);
                MoveCheck = false;
            }

            else
                Wait();
        }

        else if(RanDir == -1 && BaseChk == false && TargetChk == false && AttackTiming == false && GroundChk == true) // ?쒕뜡 ??吏곸엫 : -1?쇰븣 ?쇱そ
        {
            Walk();
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            //Debug.Log("Left");
            if (MoveCheck == false)
            {
                Waits = 0.3f;
                MoveCheck = true;
                MaxPos.x = CurPos.x - 2.0f;
            }

            if (Waits > 0)
                Waits -= Time.deltaTime * 0.1f;

            if (CurPos.x > MaxPos.x)
            {
                StartCoroutine("LeftMove");
            }

            else if (Waits < 0 && CurPos.x <= MaxPos.x)
            {
                RanDir = Random.Range(-1, 2);
                MoveCheck = false;
            }

            else
                Wait();
        }
    }

    public void PlayerAttack() // 플레이어 공격
    {
        player.OnHit(this, PlayerAttackDmg);

        GameObject Effect = Instantiate(KnifeEffect, PlayerPos, transform.rotation);

        //if (PlayerPos.x - this.transform.position.x < 0)
        //    Effect.GetComponent<SpriteRenderer>().flipX = true;

        //else
        //    Effect.GetComponent<SpriteRenderer>().flipX = false;

    }

    IEnumerator RightMove()
    {
        this.gameObject.transform.Translate(MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
        yield return YieldCache.WaitForSeconds(0.01f);
    }

    IEnumerator LeftMove()
    {
        this.gameObject.transform.Translate(-MoveSpeed * Time.deltaTime, 0.0f, 0.0f);
        yield return YieldCache.WaitForSeconds(0.01f);
    }

    IEnumerator TargetFalse()
    {
        yield return YieldCache.WaitForSeconds(0.7f);
        if (Waitcool <= 0)
        {
            Waitcool = 0.5f;
            GroundChk = true;
        }
        BasePos = CurPos;
        RanDir = 0;
    }

    void Walk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
        //animator.SetBool("Wait", false);
    }

    void Run()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        /*animator.SetBool("Wait", false)*/;
    }

    void Wait()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        /*animator.SetBool("Wait", true)*/;
    }
}
