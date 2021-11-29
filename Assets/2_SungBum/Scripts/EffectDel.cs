using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ObjDestroy", 0.47f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ObjDestroy()
    {
        Destroy(this.gameObject);
    }
}
