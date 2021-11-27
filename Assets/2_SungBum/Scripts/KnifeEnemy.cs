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
        //Debug.Log(BaseDis);
    }

    public void EnemyMove()
    {
        if(TargetChk == true && AttackTiming == false) // 가시거리내 player가 있을때 움직이고, 사정 거리내 있으면 멈춤
        {
            //Debug.Log("Attack");
            TargetMove = PlayerPos.x - CurPos.x;

            if (TargetMove < 0) this.gameObject.transform.Translate(-MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);

            else this.gameObject.transform.Translate(MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);
        }

        if ((BaseDis > MaxBaseDis || BaseChk == true) && TargetChk == false && AttackTiming == false) //스폰 위치에서 멀리 떨어졌을때 원 자리로 돌아감
        {
            //Debug.Log("Target");
            BaseChk = true;
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, BasePos, 1.3f * Time.deltaTime); 

            if (BaseDis < 0.3) // 다시 베이스 위치로 왔을 때 랜덤 이동
            {
                BaseChk = false;
                RanDir = Random.Range(-1, 2);
            }
        }

        if (RanDir == 0 && BaseChk == false && TargetChk == false && AttackTiming == false) // 랜덤 움직임 : 0일때 가만히
        {
            //Debug.Log("Wait");
            if (Waits > 0)
                Waits -= Time.deltaTime * 0.1f;

            else if(Waits <= 0)
            {
                Waits = 0.2f;
                RanDir = Random.Range(-1, 2);
            }
        }

        else if(RanDir == 1 && BaseChk == false && TargetChk == false && AttackTiming == false) // 랜덤 움직임 : 1일때 오른쪽
        {
            //Debug.Log("Right");
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
        }

        else if(RanDir == -1 && BaseChk == false && TargetChk == false && AttackTiming == false) // 랜덤 움직임 : -1일때 왼쪽
        {
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
        }
    }

    public void PlayerAttack() // 플레이어 공격
    {
        player.OnHit(this, 20);
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
}
