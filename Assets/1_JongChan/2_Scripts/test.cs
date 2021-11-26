using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Broadcaster.SendEvent(EventName.HI, TypeOfMessage.requireReceiver);
    }

    IEnumerator hi()
    {
        yield return YieldCache.WaitForSeconds(1f);
    }
}
