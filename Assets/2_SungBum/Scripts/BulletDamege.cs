using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamege : MonoBehaviour // 총알 데미지 스크립트
{
    [SerializeField] public int BulletDg = 0;
    public GameObject GunEffect;

    public Vector2 PlayerPos;

    float dy;
    float dx;
    Transform playerPos; // 원거리용 플레이어 위치
    float rotateDg; // 총알 각도

    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3.0f); // 자연 사라짐
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) // 플레이어 맞았을 때 데미지 넣고 사라짐
        {
            GameObject Effect = Instantiate(GunEffect, transform.position, transform.rotation);

            if (PlayerPos.x - this.transform.position.x < 0)
                Effect.GetComponent<SpriteRenderer>().flipX = true;

            else
                Effect.GetComponent<SpriteRenderer>().flipX = false;

            other.gameObject.GetComponent<Player>().OnHit(null, BulletDg);
            Destroy();
        }

        if(other.gameObject.name == "Tilemap")
            Destroy(this.gameObject);
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
