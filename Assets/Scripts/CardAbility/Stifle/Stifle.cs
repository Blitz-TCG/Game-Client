using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stifle : Card
{
    public float dropXP = 10f;

    public float UseStifleAbility(float xp)
    {
        float reductionAmount = xp * (dropXP / 100f);
        float actualXP = xp - reductionAmount;
        return actualXP;
    }
}
