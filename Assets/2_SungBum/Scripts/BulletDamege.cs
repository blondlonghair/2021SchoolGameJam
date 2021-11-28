using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamege : MonoBehaviour // 총알 데미지 스크립트
{
    [SerializeField] public int BulletDg = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3.0f); // 자연 사라짐
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) // 플레이어 맞았을 때 데미지 넣고 사라짐
        {
            other.gameObject.GetComponent<Player>().OnHit(null, BulletDg);
            Destroy();
        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
