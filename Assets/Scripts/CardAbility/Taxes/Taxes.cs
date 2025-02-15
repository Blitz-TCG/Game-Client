using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxes : Card
{
    public float spendMoreGold = 10f;

    public float UseTaxesAbility(float gold)
    {
        float increasedGold = gold * (spendMoreGold / 100f);
        return increasedGold;
    }
}
