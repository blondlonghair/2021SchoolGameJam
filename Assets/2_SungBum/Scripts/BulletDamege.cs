using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamege : MonoBehaviour // �Ѿ� ������ ��ũ��Ʈ
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3.0f); // �ڿ� �����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) // �÷��̾� �¾��� �� ������ �ְ� �����
        {
            other.gameObject.GetComponent<Player>().OnHit(null, 20);
            Destroy();
        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}