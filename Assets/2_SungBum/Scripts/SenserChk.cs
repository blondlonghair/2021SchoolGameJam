using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenserChk : MonoBehaviour // �ٴھ����� �߷� ����
{
    GameObject Enemy; // �θ� ��ü

    // Start is called before the first frame update
    void Start()
    {
        Enemy = transform.parent.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Tilemap")
            Enemy.GetComponent<KnifeEnemy>().GroundChk = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
