using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Actor
{
    public enum Kind
    {
        None,
        Player,
        Monster
    }

    public Kind kind; //인스팩터에서 세팅
    
    [Header("Hp")]
    public float curHp;
    public float maxHp;

    public virtual void OnHit(Unit attacker, int power)
    {
        if (CheckUnit())
        {
            curHp -= power;
        }
    }

    protected virtual bool CheckUnit()
    {
        return true;
    }
}
