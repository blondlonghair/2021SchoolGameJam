using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamege : MonoBehaviour // �Ѿ� ������ ��ũ��Ʈ
{
    [SerializeField] public int BulletDg = 0;
    public GameObject GunEffect;

    public Vector2 PlayerPos;

    float dy;
    float dx;
    Transform playerPos; // ���Ÿ��� �÷��̾� ��ġ
    float rotateDg; // �Ѿ� ����

    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3.0f); // �ڿ� �����
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) // �÷��̾� �¾��� �� ������ �ְ� �����
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
