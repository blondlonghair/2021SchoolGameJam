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

        if (this.gameObject.tag == "KnifeEnemy") // ±ÙÁ¢ ¾î±×·Î ¹üÀ§
            MaxPlayerDis = 4;

        else if(this.gameObject.tag == "GunEnemy") // ¿ø°Å¸® ¾î±×·Î ¹üÀ§
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

        if (PlayerDis < MaxPlayerDis) // ¾î±×·Î ²ø·ÈÀ» ¶§ 
            TargetChk = true;

        else if(PlayerDis > MaxPlayerDis && TargetChk == true) // ¾î±×·Î Ç®·ÈÀ» ¶§ ´Ù½Ã º£ÀÌ½º À§Ä¡ Àâ¾ÆÁÜ
        {
            //Debug.Log("TargetFalse");
            BasePos = CurPos;
            TargetChk = false;
        }
        //Debug.Log(BaseDis);
    }

    public void EnemyMove()
    {
        if(TargetChk == true && AttackTiming == false) // ê°€ì‹œê±°ë¦¬ë‚´ playerê°€ ìˆì„ë•Œ ì›€ì§ì´ê³ , ì‚¬ì • ê±°ë¦¬ë‚´ ìˆìœ¼ë©´ ë©ˆì¶¤
        {
            //Debug.Log("Attack");
            TargetMove = PlayerPos.x - CurPos.x;

            if (TargetMove < 0) this.gameObject.transform.Translate(-MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);

            else this.gameObject.transform.Translate(MoveSpeed * 2.5f * Time.deltaTime, 0.0f, 0.0f);
        }

        if ((BaseDis > MaxBaseDis || BaseChk == true) && TargetChk == false && AttackTiming == false) //ìŠ¤í° ìœ„ì¹˜ì—ì„œ ë©€ë¦¬ ë–¨ì–´ì¡Œì„ë•Œ ì› ìë¦¬ë¡œ ëŒì•„ê°
        {
            //Debug.Log("Target");
            BaseChk = true;
            this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, BasePos, 1.3f * Time.deltaTime); 

            if (BaseDis < 0.3) // ´Ù½Ã º£ÀÌ½º À§Ä¡·Î ¿ÔÀ» ¶§ ·£´ı ÀÌµ¿
            {
                BaseChk = false;
                RanDir = Random.Range(-1, 2);
            }
        }

        if (RanDir == 0 && BaseChk == false && TargetChk == false && AttackTiming == false) // ëœë¤ ì›€ì§ì„ : 0ì¼ë•Œ ê°€ë§Œíˆ
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

        else if(RanDir == 1 && BaseChk == false && TargetChk == false && AttackTiming == false) // ëœë¤ ì›€ì§ì„ : 1ì¼ë•Œ ì˜¤ë¥¸ìª½
        {
            //Debug.Log("Right");
            if (MoveCheck == false) // ÀÌµ¿ À§Ä¡ Á¤ÇØÁÜ
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

            else if(Waits < 0 && CurPos.x >= MaxPos.x) // ´Ù ¿òÁ÷¿´À» ¶§ ´Ù½Ã ·£´ı
            {
                RanDir = Random.Range(-1, 2);
                MoveCheck = false;
            }
        }

        else if(RanDir == -1 && BaseChk == false && TargetChk == false && AttackTiming == false) // ëœë¤ ì›€ì§ì„ : -1ì¼ë•Œ ì™¼ìª½
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

    public void PlayerAttack() // ÇÃ·¹ÀÌ¾î °ø°İ
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
