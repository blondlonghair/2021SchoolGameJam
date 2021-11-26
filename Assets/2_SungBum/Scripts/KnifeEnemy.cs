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
    public int MaxPlayerDis = 6;
    
    public float TargetMove;

    private void Awake()
    {
        BasePosMax.x = this.gameObject.transform.position.x + 3.5f;
        BasePosMin.x = this.gameObject.transform.position.x - 3.5f;
        BasePos = this.gameObject.transform.position;

        CurPos = this.gameObject.transform.position;

        Player = GameObject.FindWithTag("Player");
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

        if (PlayerDis < MaxPlayerDis)
            TargetChk = true;

        else if(PlayerDis > MaxPlayerDis && TargetChk == true)
        {
            BasePos = CurPos;
            TargetChk = false;
        }
        //Debug.Log(BaseDis);
    }

    public void EnemyMove()
    {
        if(TargetChk == true && AttackTiming == false) // 가시거리내 player가 있을때 움직이고, 사정 거리내 있으면 멈춤
        {
            TargetMove = PlayerPos.x - CurPos.x;

            if (TargetMove < 0) this.gameObject.transform.Translate(-4.8f * Time.deltaTime, 0.0f, 0.0f);

            else this.gameObject.transform.Translate(4.8f * Time.deltaTime, 0.0f, 0.0f);
        }

        if (BaseDis > MaxBaseDis || BaseChk == true && TargetChk == false) //스폰 위치에서 멀리 떨어졌을때 원 자리로 돌아감
        {
            BaseChk = true;
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, BasePos, 1.2f * Time.deltaTime);

            if (BaseDis < 0.3)
            {
                BaseChk = false;
                RanDir = Random.Range(-1, 2);
            }
        }

        if (RanDir == 0 && BaseChk == false && TargetChk == false) // 랜덤 움직임 : 0일때 가만히
        {
            //Debug.Log("Wait");
            if(Waits > 0)
                Waits -= Time.deltaTime * 0.1f;

            else if(Waits <= 0)
            {
                Waits = 0.2f;
                RanDir = Random.Range(-1, 2);
            }
        }

        else if(RanDir == 1 && BaseChk == false && TargetChk == false) // 랜덤 움직임 : 1일때 오른쪽
        {
            //Debug.Log("Right");
            if (MoveCheck == false)
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

            else if(Waits < 0 && CurPos.x >= MaxPos.x)
            {
                RanDir = Random.Range(-1, 2);
                MoveCheck = false;
            }
        }

        else if(RanDir == -1 && BaseChk == false && TargetChk == false) // 랜덤 움직임 : -1일때 왼쪽
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

    IEnumerator RightMove()
    {
        this.gameObject.transform.Translate(2.0f * Time.deltaTime, 0.0f, 0.0f);
        yield return YieldCache.WaitForSeconds(0.01f);
    }

    IEnumerator LeftMove()
    {
        this.gameObject.transform.Translate(-2.0f * Time.deltaTime, 0.0f, 0.0f);
        yield return YieldCache.WaitForSeconds(0.01f);
    }
}
