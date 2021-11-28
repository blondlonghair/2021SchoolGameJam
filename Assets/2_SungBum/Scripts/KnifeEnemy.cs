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

    public Animator animator;

    private void Awake()
    {
        BasePosMax.x = this.gameObject.transform.position.x + 3.5f;
        BasePosMin.x = this.gameObject.transform.position.x - 3.5f;
        BasePos = this.gameObject.transform.position;

        CurPos = this.gameObject.transform.position;

        Player = GameObject.FindWithTag("Player");
        player = Player.GetComponent<Player>();

        if (this.gameObject.tag == "KnifeEnemy") // ±Ÿ¡¢ æÓ±◊∑Œ π¸¿ß
            MaxPlayerDis = 4;

        else if(this.gameObject.tag == "GunEnemy") // ø¯∞≈∏Æ æÓ±◊∑Œ π¸¿ß
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

        if (PlayerDis < MaxPlayerDis) // æÓ±◊∑Œ ≤¯∑»¿ª ∂ß 
            TargetChk = true;

        else if(PlayerDis > MaxPlayerDis && TargetChk == true) // æÓ±◊∑Œ «Æ∑»¿ª ∂ß ¥ŸΩ√ ∫£¿ÃΩ∫ ¿ßƒ° ¿‚æ∆¡‹
        {
            //Debug.Log("TargetFalse");
            BasePos = CurPos;
            TargetChk = false;
        }
        //Debug.Log(BaseDis);
    }

    public void EnemyMove()
    {
        if(TargetChk == true && AttackTiming == false) // Í∞Ä?úÍ±∞Î¶¨ÎÇ¥ playerÍ∞Ä ?àÏùÑ???ÄÏßÅÏù¥Í≥? ?¨Ï†ï Í±∞Î¶¨???àÏúºÎ©?Î©àÏ∂§
        {
            Run();
            //Debug.Log("Attack");
            TargetMove = PlayerPos.x - CurPos.x;

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

        if ((BaseDis > MaxBaseDis || BaseChk == true) && TargetChk == false && AttackTiming == false) //?§Ìè∞ ?ÑÏπò?êÏÑú Î©ÄÎ¶??®Ïñ¥Ï°åÏùÑ?????êÎ¶¨Î°??åÏïÑÍ∞?
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

            if (BaseDis < 0.3) // ¥ŸΩ√ ∫£¿ÃΩ∫ ¿ßƒ°∑Œ ø‘¿ª ∂ß ∑£¥˝ ¿Ãµø
            {
                BaseChk = false;
                RanDir = Random.Range(-1, 2);
            }
        }

        if (RanDir == 0 && BaseChk == false && TargetChk == false && AttackTiming == false) // ?úÎç§ ?ÄÏßÅÏûÑ : 0?ºÎïå Í∞ÄÎßåÌûà
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

        else if(RanDir == 1 && BaseChk == false && TargetChk == false && AttackTiming == false) // ?úÎç§ ?ÄÏßÅÏûÑ : 1?ºÎïå ?§Î•∏Ï™?
        {
            Walk();
            //Debug.Log("Right");
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            if (MoveCheck == false) // ¿Ãµø ¿ßƒ° ¡§«ÿ¡‹
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

            else if(Waits < 0 && CurPos.x >= MaxPos.x) // ¥Ÿ øÚ¡˜ø¥¿ª ∂ß ¥ŸΩ√ ∑£¥˝
            {
                RanDir = Random.Range(-1, 2);
                MoveCheck = false;
            }

            else
                Wait();
        }

        else if(RanDir == -1 && BaseChk == false && TargetChk == false && AttackTiming == false) // ?úÎç§ ?ÄÏßÅÏûÑ : -1?ºÎïå ?ºÏ™Ω
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

    public void PlayerAttack() // «√∑π¿ÃæÓ ∞¯∞›
    {
        player.OnHit(this, PlayerAttackDmg);
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

    void Walk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
        animator.SetBool("wait", false);
    }

    void Run()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        animator.SetBool("wait", false);
    }

    void Wait()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("wait", true);
    }
}
