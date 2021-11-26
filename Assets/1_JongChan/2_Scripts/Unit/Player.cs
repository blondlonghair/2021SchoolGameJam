using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Player : Unit
{
    public void hi()
    {
        print("hi");
    }

    protected override void OnHit(Unit attacker, int power)
    {
        base.OnHit(attacker, power);
    }

    protected override bool CheckUnit()
    {
        return base.CheckUnit();
    }
}
