using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenserChk : MonoBehaviour // ¹Ù´Ú¾øÀ¸¸é Áß·Â »ý±è
{
    GameObject Enemy; // ºÎ¸ð °´Ã¼

    bool GroundChk = true;
    public float cool = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = transform.parent.gameObject;
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("d");
    //        Enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    //    }
    //}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if(cool <= 0.0f)
            {
                cool = 2.0f; 
                Debug.Log("d");
                Enemy.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                Enemy.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("d");
            Enemy.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cool >= 0.0f)
            cool -= Time.deltaTime * 1;
    }
}
